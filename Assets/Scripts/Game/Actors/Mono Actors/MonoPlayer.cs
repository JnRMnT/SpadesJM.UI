using UnityEngine;
using System.Collections;
using Game.Actors;

public class MonoPlayer : MonoBehaviour {
    public Transform activeCard;
    public UICard activeCardScript;
    public UIDeck playerDeck;
    public Transform splitter;
    public TurnTimeoutHandler TurnTimeoutHandler;
    public SoundManager SoundManager;
    public Game.Actors.Player player;

	void Start () {
        
	}

    public void InitializePlayer(GameTable gameTable)
    {
        player = new UIPlayer(gameTable,this);
    }

    public void Test(string message)
    {
        LogManager.Log(message);
    }

	// Update is called once per frame
	void Update () {
	
	}

    public Game.Actors.Player getInternalPlayer()
    {
        return player;
    }
}
