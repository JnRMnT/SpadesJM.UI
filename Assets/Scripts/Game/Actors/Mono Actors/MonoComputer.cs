using UnityEngine;
using System.Collections;

public class MonoComputer : MonoBehaviour {

    public UIComputer computer;
    public UIGameTable gameTable;
    public TurnTimeoutHandler TurnTimeoutHandler;

    private float counter;
    private bool count;

	// Use this for initialization
	void Start () {
        count = false;
        counter = 0f;
	}

    public UIComputer InitializeComputer()
    {
        computer = new UIComputer(gameTable.gameTable,this);
        return computer;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (count)
        {
            counter += Time.deltaTime;
            if (counter >= 1.5f)
            {
                count = false;
                LogManager.Log("Doing Action");
                computer.DoAction();
                counter = 0f;
            }
        }
	}

    public void WaitForAction()
    {
        count = true;
    }


}
