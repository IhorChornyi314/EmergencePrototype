using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class StressTest : NetworkBehaviour
{
    public NetworkObject stressPrefab;
    public int number;
    private int _iterationsLeft = 50;

    [ServerRpc]
    public void StressTestServerRpc(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 position = new Vector3((Random.value - 0.5f) * 200, (Random.value - 0.5f) * 200);
            NetworkObject testUnit = Instantiate(stressPrefab, position, Quaternion.identity);
            testUnit.SpawnWithOwnership(NetworkManager.Singleton.ConnectedClientsIds[0]);
        }
    }
    

    public void Update()
    {
        if (!NetworkManager.Singleton.IsServer && _iterationsLeft > 0)
        {
            StressTestServerRpc(number);
            _iterationsLeft--;
        }
    }
}
