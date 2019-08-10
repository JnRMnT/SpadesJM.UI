using UnityEngine;
using System.Collections;
using Common;

public class MonoNetworkPlayer : MonoNetworkPlayerBase
{
    public SoundManager SoundManager;
    public override NetworkPlayerBase InitializeNetworkPlayer()
    {
        player = new LocalNetworkPlayer(gameTable.gameTable, this);
        return player;
    }
}
