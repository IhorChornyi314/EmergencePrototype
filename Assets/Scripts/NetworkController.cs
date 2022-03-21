using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using TMPro;
using UnityEngine;

public class NetworkController : NetworkBehaviour
{
    public List<GameObject> playerObjects;

    public void Awake()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnect;
    }

    private void HandleClientConnect(ulong clientId)
    {
        playerObjects[0].SetActive(true);
    }

    // [ServerRpc]
    // public void SpawnNetworkObjectServerRpc(ulong clientID, NetworkObject networkObject, Vector2 position)
    // {
    //     NetworkObject newObject = Instantiate(networkObject, position, Quaternion.identity);
    //     newObject.SpawnWithOwnership(clientID);
    // }
    //
    // public void SpawnNetworkObject(NetworkObject networkObject, Vector2 position)
    // {
    //     SpawnNetworkObjectServerRpc(NetworkManager.Singleton.LocalClientId, networkObject, position);
    // }
}
