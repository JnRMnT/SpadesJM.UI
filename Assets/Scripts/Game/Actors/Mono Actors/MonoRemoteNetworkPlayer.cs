using UnityEngine;
using System.Collections;
using Common;

public class MonoRemoteNetworkPlayer : MonoNetworkPlayerBase
{
    public RemoteNetworkPlayer player;

    public override NetworkPlayerBase InitializeNetworkPlayer()
    {
        player = new RemoteNetworkPlayer(gameTable.gameTable, this);
        return player;
    }
}
