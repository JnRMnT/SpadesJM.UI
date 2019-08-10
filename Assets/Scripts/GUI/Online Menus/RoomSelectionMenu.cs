using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;

public class RoomSelectionMenu : MonoBehaviour {
    public GUISkin RoomSelectionGUISkin;
    private static Transform transformInstance;

    public Texture2D Background, SelectedRoomBoxBackground, Lock;

    private float offsetHeight,offsetWidth;
    private Rect scrollViewPosition,scrollViewInnerPosition;
    private Rect createRoomButtonPos,joinRoomButtonPos,renewButtonPos;
    private Vector2 scrollPosition = Vector2.zero;

    public static int RoomCount = 0;
    public List<LiveRoomInfoEvent> ActiveRooms;
    private Dictionary<string, object> selectedRoomProperties;

    private Rect roomBoxPosition;
    private Vector2 touchPosition;

    private string selectedRoomId = string.Empty;
    private string selectedRoomPw = string.Empty;

    void Start()
    {
        transformInstance = this.transform;

        offsetWidth = Screen.width / 20;
        offsetHeight = Screen.height / 40;
        scrollViewPosition = new Rect(offsetWidth / 2, offsetHeight * 2 + Properties.BackButtonSize, Screen.width - offsetWidth * 2, Screen.height - Properties.TEXT_HEIGHT/2*7 - Properties.BackButtonSize);
        scrollViewInnerPosition = new Rect(0, 0, Screen.width - offsetWidth * 3, Screen.height* 4);

        createRoomButtonPos = new Rect(Screen.width - Screen.width / 4 - offsetWidth*2 - Screen.width/5, Screen.height - Properties.TEXT_HEIGHT / 3 * 7, Screen.width / 4, Properties.TEXT_HEIGHT * 2);
        joinRoomButtonPos = new Rect(offsetWidth, Screen.height - Properties.TEXT_HEIGHT / 3 * 7, Screen.width / 3, Properties.TEXT_HEIGHT * 2);
        renewButtonPos = new Rect(Screen.width - Screen.width / 5 - offsetWidth, Screen.height - Properties.TEXT_HEIGHT / 3 * 7, Screen.width / 5, Properties.TEXT_HEIGHT * 2);

        roomBoxPosition = new Rect(0, 0, Screen.width / 7 * 3 - offsetWidth / 3, Properties.TEXT_HEIGHT * 10.5f);

        ActiveRooms = new List<LiveRoomInfoEvent>();
        Hide();
        scrollViewInnerPosition.height = offsetHeight * (Mathf.CeilToInt(RoomCount / 2) + 1) + (Mathf.CeilToInt(RoomCount / 2)+(RoomCount%2)) * roomBoxPosition.height;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
            WarpClient.GetInstance().Disconnect();
            LoginScreen.Show();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                scrollPosition.y += touch.position.y - touchPosition.y;
                touchPosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            scrollPosition.y += Input.mousePosition.y - touchPosition.y;
            touchPosition = Input.mousePosition;
        }
    }

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Show(string commandToRun)
    {
        if (commandToRun == "ROOMDLT")
        {
            DialogBox.DialogBoxButtons = DialogBoxButtons.OK;
            DialogBox.DialogText = LanguageManager.getString("ROOMDLT");
            DialogBox.Show();
            Renew();
        }
        else
        {
            Show();
        }
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);
        RoomCount = 0;
    }

    public static void Activate()
    {
        LoginResultScreen.Hide();
        LoadingScreen.Hide();
        RoomSelectionMenu.Show();
    }

    public void AddRoom(LiveRoomInfoEvent eventObj)
    {
        ActiveRooms.Add(eventObj);
        if (ActiveRooms.Count >= RoomCount)
        {
            scrollViewInnerPosition.height = offsetHeight * (RoomCount + 2) + RoomCount * roomBoxPosition.height;
            Activate();
        }
    }

    private void DrawRoomBox(string roomName,string type,string goal,string players,int playerCount,string pw)
    {
        float offsetY = roomBoxPosition.height / 20;
        float offsetX = roomBoxPosition.width / 20;

        GUI.Box(new Rect(offsetX/2, offsetY, roomBoxPosition.width - offsetX, Properties.TEXT_HEIGHT * 2), "");
        GUI.Label(new Rect(offsetX, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT*2), LanguageManager.getString("ROOMNM")+":");
        GUI.Label(new Rect(roomBoxPosition.width / 2, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT * 2), roomName);

        offsetY += roomBoxPosition.height / 40 + Properties.TEXT_HEIGHT * 2;
        GUI.Box(new Rect(offsetX / 2, offsetY, roomBoxPosition.width - offsetX, Properties.TEXT_HEIGHT / 4 * 5), "");
        GUI.Label(new Rect(offsetX, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT / 4 * 5), LanguageManager.getString("PLYRCNT") + ":");
        GUI.Label(new Rect(roomBoxPosition.width / 2, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT / 4 * 5), playerCount.ToString());

        if (!string.IsNullOrEmpty(pw))
        {
            GUI.DrawTexture(new Rect(roomBoxPosition.width - offsetX * 5, offsetY + Properties.TEXT_HEIGHT / 4, Properties.TEXT_HEIGHT / 4 * 3, Properties.TEXT_HEIGHT / 4 * 3), Lock);
        }

        offsetY += roomBoxPosition.height / 40 + Properties.TEXT_HEIGHT / 4 * 5;
        GUI.Box(new Rect(offsetX / 2, offsetY, roomBoxPosition.width - offsetX, Properties.TEXT_HEIGHT * 4), "");
        GUI.Label(new Rect(offsetX, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT * 4), LanguageManager.getString("PLYRS") + ":");
        GUI.Label(new Rect(roomBoxPosition.width / 2, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT * 4), players);

        offsetY += roomBoxPosition.height / 40 + Properties.TEXT_HEIGHT * 4;
        GUI.Box(new Rect(offsetX / 2, offsetY, roomBoxPosition.width - offsetX, Properties.TEXT_HEIGHT / 5 * 7), "");
        GUI.Label(new Rect(offsetX, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT / 5 * 7), LanguageManager.getString("GMENDNG") + ":");
        string endCondition = string.Empty;
        if (type == "HNDCNT")
        {
            endCondition = LanguageManager.getString("ROOMENDHND");
        }
        else
        {
            endCondition = LanguageManager.getString("ROOMENDSCR");
        }
        GUI.Label(new Rect(roomBoxPosition.width / 2, offsetY, roomBoxPosition.width / 2 - offsetX * 2, Properties.TEXT_HEIGHT / 5 * 7), goal+" "+endCondition);
    }
    
    void OnGUI()
    {
        GUI.depth = 0;
        GUI.skin = RoomSelectionGUISkin;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Background);

        if (GUI.Button(new Rect(Screen.width / 25, Screen.height / 25, Properties.BackButtonSize, Properties.BackButtonSize), Properties.BackButton))
        {
            Hide();
            WarpClient.GetInstance().Disconnect();
            LoginScreen.Show();
        }

        scrollPosition = GUI.BeginScrollView(scrollViewPosition, scrollPosition, scrollViewInnerPosition);

        float offsetX = roomBoxPosition.x;
        float offsetY = roomBoxPosition.y;
        for (int i = 0; i < ActiveRooms.Count; i++)
        {
            LiveRoomInfoEvent currentRoom = ActiveRooms[i];
            string[] joinedUsers = currentRoom.getJoinedUsers();
            string roomId = currentRoom.getData().getId();
            object type = string.Empty;
            object goal = string.Empty;
            object pw = string.Empty;
            selectedRoomProperties = currentRoom.getProperties();
            selectedRoomProperties.TryGetValue("PW", out pw);
            if (selectedRoomProperties.TryGetValue("TYPE", out type) && selectedRoomProperties.TryGetValue("GOAL", out goal))
            {
                string playersText = string.Empty;

                for (int j = 0; j < 4; j++)
                {
                    if (j == 0 && joinedUsers != null)
                    {
                        playersText = joinedUsers[j];
                    }
                    else if (j == 0)
                    {
                        playersText = "-";
                    }
                    else if (joinedUsers != null && j < joinedUsers.Length)
                    {
                        playersText += "\n" + joinedUsers[j];
                    }
                    else
                    {
                        playersText += "\n-";
                    }
                }

                if (selectedRoomId == roomId)
                {
                    GUI.DrawTexture(new Rect(offsetX, offsetY, roomBoxPosition.width, roomBoxPosition.height), SelectedRoomBoxBackground);
                }
                else if (GUI.Button(new Rect(offsetX, offsetY, roomBoxPosition.width, roomBoxPosition.height), ""))
                {
                    selectedRoomId = roomId;
                    selectedRoomPw = pw == null ? null : pw.ToString();
                }
                GUI.BeginGroup(new Rect(offsetX, offsetY, roomBoxPosition.width, roomBoxPosition.height));
                DrawRoomBox(currentRoom.getData().getName(), type.ToString(), goal.ToString(), playersText, joinedUsers == null ? 0 : joinedUsers.Length,pw == null? null : pw.ToString());
                GUI.EndGroup();
                offsetX += offsetWidth / 2 + roomBoxPosition.width;
                if (i % 2 == 1)
                {
                    offsetY += offsetHeight + roomBoxPosition.height;
                    offsetX = roomBoxPosition.x;
                }
            }
        }

        GUI.EndScrollView();

        if (GUI.Button(createRoomButtonPos, LanguageManager.getString("CRTROOM")))
        {
            Hide();
            CreateRoomMenu.Show();
        }

        if (!string.IsNullOrEmpty(selectedRoomId) && GUI.Button(joinRoomButtonPos, LanguageManager.getString("JOINROOM")))
        {
            if (selectedRoomPw != null && !string.IsNullOrEmpty(selectedRoomPw.ToString()))
            {
                PasswordEnterMenu.Show(selectedRoomPw.ToString(), selectedRoomId);
                Hide();
            }
            else
            {
                StartCoroutine(JoinRoom(selectedRoomId));
            }
        }

        if (GUI.Button(renewButtonPos, LanguageManager.getString("RENEW")))
        {
            Renew();
        }
    }

    public static void Renew()
    {
        LoginResultScreen.Show();
        LoadingScreen.Show();
        LoginResultScreen.RedirectToRoomSelection();
    }

    public IEnumerator JoinRoom(string roomId)
    {
        JoinRoomResultScreen.Show();
        JoinRoomResultScreen.JoinResult = -2;
        JoinRoomResultScreen.RoomId = roomId;
        LobbyPlayerStats.RoomProperties = selectedRoomProperties;
        MultiplayerListener.ActivePage = 2;
        yield return null;
        WarpClient.GetInstance().SubscribeRoom(roomId);
        WarpClient.GetInstance().JoinRoom(roomId);
        Hide();
    }

    public void DoJoin(string selectedRoomId)
    {
        StartCoroutine(JoinRoom(selectedRoomId));
    }
}
