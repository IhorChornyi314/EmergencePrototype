using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public List<Vector3> startingPositions;

    public async void WaitForTeam()
    {
        while (ServerManager.Instance.team == -1)
        {
            await Task.Delay(100);
        }
        int team = ServerManager.Instance.team;
        SpawnManager.Instance.SpawnEntityServerRpc("Spawner", startingPositions[team], team);
    }

    public override void OnNetworkSpawn()
    {
        WaitForTeam();
    }
}
