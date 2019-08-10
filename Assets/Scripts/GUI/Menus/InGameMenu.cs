using UnityEngine;
using System.Collections;
using Assets.Scripts.System;
using com.shephertz.app42.gaming.multiplayer.client;

public class InGameMenu : MonoBehaviour
{
    private static Transform transformInstance;
    private Rect groupPosition;
    private float offsetX, offsetY;

    public MonoNetworkPlayer Player;

    public GUISkin InGameMenuSkin;

    public static void Show()
    {
        transformInstance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        transformInstance.gameObject.SetActive(false);

    }

    void Start()
    {
        transformInstance = transform;

        groupPosition = new Rect(Screen.width / 8, Screen.height / 8, Screen.width - Screen.width / 4, Screen.height - Screen.height / 4);

        offsetX = groupPosition.width / 8;
        offsetY = groupPosition.height / 10;

        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void DialogBoxCallback(bool answer)
    {
        if (Properties.ActiveGameType == GameType.SinglePlayer)
        {
            if (answer)
            {
                Hide();
                PlayerStats.Hide();
                LobbyPlayerStats.Hide();
                MainMenu.Show();
            }
            else
            {
                Hide();
            }
        }
        else
        {
            if (answer)
            {
                WarpClient.GetInstance().LeaveRoom(LobbyPlayerStats.RoomData.getId());
                if (LobbyPlayerStats.RoomData.getRoomOwner() == Player.GetInternalPlayer().PlayerName)
                {
                    string roomDeleteCommand = "ROOMDLT";
                    MultiplayerManager.SendBytes(ByteHelper.GetBytes(roomDeleteCommand));
                    WarpClient.GetInstance().DeleteRoom(LobbyPlayerStats.RoomData.getId());
                    RoomSelectionMenu.Show(roomDeleteCommand);
                }
                else
                {
                    RoomSelectionMenu.Renew();
                }
                LobbyPlayerStats.RoomData = null;
                PlayerStats.Hide();
                LobbyPlayerStats.Hide();
                Hide();
            }
        }
    }


    void OnGUI()
    {
        if (LanguageManager.IsLoaded)
        {
            GUI.skin = InGameMenuSkin;
            GUI.depth = 1;

            GUI.Box(groupPosition, "");
            GUI.BeginGroup(groupPosition);

            float offsetY = this.offsetY * 2;
            if (GUI.Button(new Rect(offsetX, offsetY, groupPosition.width - offsetX * 2, Properties.TEXT_HEIGHT * 3 / 2), LanguageManager.getString("CNTNE")))
            {
                Hide();
            }

            offsetY += Properties.TEXT_HEIGHT + this.offsetY;
            if (GUI.Button(new Rect(offsetX, offsetY, groupPosition.width - offsetX * 2, Properties.TEXT_HEIGHT * 3 / 2), LanguageManager.getString("STNGS")))
            {
                Hide();
                SettingsMenu.BackMenu = this.GetType();
                SettingsMenu.Show();
            }

            offsetY += Properties.TEXT_HEIGHT + this.offsetY;
            if (GUI.Button(new Rect(offsetX, offsetY, groupPosition.width - offsetX * 2, Properties.TEXT_HEIGHT * 3 / 2), LanguageManager.getString("LEAVEGM")))
            {
                DialogBox.DialogBoxButtons = DialogBoxButtons.YES_NO;
                DialogBox.DialogText = LanguageManager.getString("PROMPT_LEAVEGM");
                DialogBox.CallbackObject = gameObject;
                DialogBox.Show();
            }

            GUI.EndGroup();
        }
    }
}
