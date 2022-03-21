using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Netcode;
using UnityEngine;

public class ServerManager : NetworkBehaviour
{
    public Queue<OrderPacket> LatestOrders = new Queue<OrderPacket>();
    public static ServerManager Instance;

    public Dictionary<ulong, int> playerTeams = new Dictionary<ulong, int>();
    public int team = -1;

    public void Awake()
    {
        Instance = this;
    }
    
    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.NetworkTickSystem.Tick += Tick;
            NetworkManager.Singleton.OnClientConnectedCallback += RegisterClient;
        }
    }

    public void RegisterClient(ulong clientID)
    {
        SetTeamClientRpc(playerTeams.Count);
        playerTeams.Add(clientID, playerTeams.Count);
    }

    [ClientRpc]
    public void SetTeamClientRpc(int team) => this.team = team;

    private void Tick()
    {
        if (LatestOrders.Count > 0)
        {
            foreach (var latestOrderPacket in LatestOrders)
            {
                Order latestOrder = latestOrderPacket.Order;
                if (latestOrder != null)
                {
                    EntityManager.Instance.AddOrResetOrders(latestOrder, latestOrderPacket.Reset, latestOrderPacket.EntityIds);
                    var stream = new MemoryStream();
                    var serializer = new BinaryFormatter();
                    serializer.Serialize(stream, latestOrder);
                    SendPacketsToClientRpc(latestOrderPacket.Reset, latestOrderPacket.EntityIds, stream.ToArray());
                }
            }

            LatestOrders.Clear();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendOrdersToServerRpc(ulong clientID, bool reset, int[] entityIDs, byte[] orderBytes)
    {
        //TODO: Check for client validity
        var serializer = new BinaryFormatter();
        Order order = (Order)serializer.Deserialize(new MemoryStream(orderBytes));
        order.Deserialize();
        OrderPacket orderPacket = new OrderPacket(reset, entityIDs, order);
        LatestOrders.Enqueue(orderPacket);
    }
    
    [ClientRpc]
    public void SendPacketsToClientRpc(bool reset, int[] entityIDs, byte[] orderBytes)
    {
        Debug.Log("!");
        if (entityIDs == null)
        {
            return;
        }
        var serializer = new BinaryFormatter();
        Order order = (Order)serializer.Deserialize(new MemoryStream(orderBytes));
        order.Deserialize();
        EntityManager.Instance.AddOrResetOrders(order, reset, entityIDs);
    }

    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.NetworkTickSystem.Tick -= Tick;
            NetworkManager.Singleton.OnClientConnectedCallback -= RegisterClient;
        }
    }
}
