using UnityEngine;
using System.Collections;

public class MonoNetworkPlayerBase : MonoBehaviour {

    public NetworkPlayerBase player;
    public UIGameTable gameTable;
    public TurnTimeoutHandler TurnTimeoutHandler;

    public virtual NetworkPlayerBase InitializeNetworkPlayer()
    {
        return null;
    }

    // Use this for initialization
    void Start()
    {
        InitializeNetworkPlayer();
        TurnTimeoutHandler = GameObject.FindGameObjectWithTag("SCRIPTWRAPPER").GetComponent<TurnTimeoutHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public NetworkPlayerBase GetInternalPlayer()
    {
        return player;
    }
}
