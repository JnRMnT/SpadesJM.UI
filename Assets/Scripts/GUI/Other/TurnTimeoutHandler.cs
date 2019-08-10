using UnityEngine;
using System.Collections;
using Game.Actors;
using System.Reflection;
using Common.Enums;
using Assets.Scripts.System;

public class TurnTimeoutHandler : MonoBehaviour {
    private Player activePlayer;
    private ActionType awaitingAction;
    private object callbackObject;
    private float timer;
    public float TurnTimer = 60f;
    private bool timerActive = false;
    public SoundManager SoundManager;

    public GUISkin TurnTimerSkin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timerActive && (LobbyPlayerStats.IsShown || PlayerStats.IsShown))
        {
            timer -= Time.fixedDeltaTime;

            if (timer <= 10)
            {
                SoundManager.PlayAboutToTimeoutSound();
            }

            if (timer <= 0)
            {
                timerActive = false;
                timer = TurnTimer;
				activePlayer.HandleActionTimeout(awaitingAction,callbackObject);
                SoundManager.StopAboutToTimeoutSound();
            }
        }
	}

    public void StartTurnTimer(Player player,ActionType awaitingAction,object callbackObject)
    {
        if (activePlayer != player)
        {
            timer = TurnTimer;
            this.activePlayer = player;
            this.awaitingAction = awaitingAction;
            this.callbackObject = callbackObject;
            timerActive = true;

            SoundManager.StopAboutToTimeoutSound();
        }
    }

    public void ResetTurnTimer()
    {
        this.timer = TurnTimer;
        this.activePlayer = null;
        this.timerActive = false;

        SoundManager.StopAboutToTimeoutSound();
    }

    void OnGUI()
    {
        if (timerActive)
        {
            GUI.skin = TurnTimerSkin;
            GUI.depth = 20;

            GUI.Box(new Rect(Screen.width / 40 + Screen.width / 9, Screen.height / 40, Screen.width / 9, Screen.width / 9), ((int)timer).ToString());
            
        }
    }
}
