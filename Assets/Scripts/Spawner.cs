using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : Building
{
    [SerializeField] private string unitName;
    [SerializeField] private Vector3 offset;
    private float _elapsedTime;

    private NetworkVariable<int> _team = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            _team.Value = ServerManager.Instance.team;
        }

    }

    public void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= ((BuildingData)data).effectCooldown)
        {
            _elapsedTime = 0;
            Vector3 position = new Vector3(Random.value, 0) + transform.position + offset;
                
            SpawnManager.Instance.SpawnLocalEntity(unitName, position, _team.Value, -1, SpawnOrders);
        }
    }

    
}
