using UnityEngine;
using System.Collections;

public class TurnArrowController : MonoBehaviour {

    public static Transform Top, Bot, Right, Left;
    private static Transform activeArrow;

	// Use this for initialization
	void Start () {
        Top = transform.FindChild("TurnArrowTop");
        Bot = transform.FindChild("TurnArrowBot");
        Left = transform.FindChild("TurnArrowLeft");
        Right = transform.FindChild("TurnArrowRight");

        Top.gameObject.SetActive(false);
        Bot.gameObject.SetActive(false);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetActive(int player)
    {
        TurnArrowOff();
        
        Transform arrow = null;
        switch (player)
        {
            case 0:
                arrow = Bot;
                break;
            case 1:
                arrow = Right;
                break;

            case 2:
                arrow = Top;
                break;

            case 3:
                arrow = Left;
                break;
        }

        arrow.gameObject.SetActive(true);
        activeArrow = arrow;
    }

    public static void TurnArrowOff()
    {
        if (activeArrow != null)
        {
            activeArrow.gameObject.SetActive(false);
        }
    }


}
