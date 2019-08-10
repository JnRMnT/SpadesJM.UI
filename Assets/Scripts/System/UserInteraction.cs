using UnityEngine;
using System.Collections;

public class UserInteraction : MonoBehaviour {
    public MonoPlayer player;
    public static bool InputActive = false;
    public UIGameTable GameTable;
    public static bool InGame = false;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

	// Update is called once per frame
	void Update () {

        if (InputActive)
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput(Input.mousePosition);
            }
            else if (Input.touchCount > 0)
            {
                HandleInput(Input.GetTouch(0).position);
            }
            else
            {
                if (player.activeCard != null)
                {
                    player.activeCard.SendMessage("Release");
                    player.splitter.gameObject.SetActive(false);
                    player.activeCard = null;
                }
            }
        }
	}

    void HandleInput(Vector3 tapPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (player.activeCard != null)
            {
                player.activeCardScript.HandleTap(Camera.main.ScreenToWorldPoint(tapPosition));
            }
            else
            {
                switch (hit.transform.tag)
                {
                    case "CARD":
                        if ((player.activeCard == null && hit.transform.position.y < player.splitter.transform.position.y))
                        {
                            player.splitter.gameObject.SetActive(true);
                            player.activeCard = hit.transform;
                            UICard cardScript = hit.transform.GetComponent<UICard>();
                            player.activeCardScript = cardScript;
                            cardScript.HandleTap(Camera.main.ScreenToWorldPoint(tapPosition));
                        }
                        break;
                }
            }
        }
    }
}
