using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    public static SpawnManager Instance;
    public const int MAX_UNITS = 1000;
    private Dictionary<int, int> unitsPerTeam = new Dictionary<int, int>();

    public void Awake()
    {
        Instance = this;
        InitializePrefabs();
    }
    
    public void InitializePrefabs()
    {
        GameObject[] loadedEntityPrefabs = Resources.LoadAll<GameObject>("Prefabs/Entities/");
        foreach (GameObject entityPrefab in loadedEntityPrefabs)
        {
            prefabs.Add(entityPrefab.name, entityPrefab);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnEntityServerRpc(string prefabName, Vector3 position, int team)
    {
        Instance.SpawnEntity(prefabName, position, team);
    }
    
    public int SpawnEntity(string prefabName, Vector3 position, int team, List<Order> startingOrders = null)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (!unitsPerTeam.ContainsKey(team))
                unitsPerTeam.Add(team, 0);
            if (unitsPerTeam[team] >= MAX_UNITS)
                return -1;
            int entityID = Instance.SpawnLocalEntity(prefabName, position, team, -1, startingOrders);
            Instance.SpawnEntityClientRpc(prefabName, entityID, team, position);
            unitsPerTeam[team]++;
            return entityID;
        }

        return -1;
    }
    
    [ClientRpc]
    public void SpawnEntityClientRpc(string prefabName, int entityID, int team, Vector3 position)
    {
        Instance.SpawnLocalEntity(prefabName, position, team, entityID);
    }
    
    public int SpawnLocalEntity(string prefabName, Vector3 position, int team, int entityId = -1, List<Order> startingOrders = null)
    {
        Entity newEntity = Instantiate(prefabs[prefabName], position, Quaternion.identity).GetComponent<Entity>();
        newEntity.Position = position;
        EntityManager.Instance.groups[0].AddEntities(new List<Entity>(new []{newEntity}));
        newEntity.team = team;
        int result = EntityManager.Instance.RegisterEntity(newEntity, entityId);
        if (startingOrders != null)
            foreach (Order startingOrder in startingOrders)
            {
                EntityManager.Instance.AddOrResetOrders(startingOrder, true, new []{result});
            }

        return result;
    }
}
