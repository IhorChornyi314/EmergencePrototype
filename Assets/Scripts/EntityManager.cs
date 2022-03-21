using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityManager : MonoBehaviour
{
    
    public List<EntityGroup> groups;
    public List<Entity> entities;
    public List<Order> currentOrders;
    public int localPlayerTeam;
    public static EntityManager Instance;


    public void Awake()
    {
        Instance = this;
        groups = new List<EntityGroup>();
        for (int i = 0; i < 10; i++)
        {
            groups.Add(new EntityGroup(new List<Entity>()));
        }
    }

    public int GetEntityID(Entity entity) => entities.IndexOf(entity);


    public Entity GetEntityAtPoint(Vector2 point)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(point);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.GetComponent<Entity>() != null)
            {
                return hit.gameObject.GetComponent<Entity>();
            }
        }

        return null;
    }


    public void SetTarget(Vector2 targetPos, bool resetOrders)
    {
        Entity targetEntity = GetEntityAtPoint(targetPos);
        if (targetEntity != null)
        {
            if (targetEntity.team == localPlayerTeam)
            {
                SendOrders(new MoveOrder(GetEntityID(targetEntity), 0.5f), resetOrders);
            }
        }
        else
        {
            SendOrders(new MoveOrder(targetPos, 0.5f), resetOrders);
        }
    }

    public void SendOrders(Order order, bool reset, int[] entityIDs = null)
    {
        var stream = new MemoryStream();
        var serializer = new BinaryFormatter();
        order.Serialize();
        serializer.Serialize(stream, order);
        if (entityIDs == null)
            entityIDs = SelectManager.Instance.selectedEntities.ToArray();
        ServerManager.Instance.SendOrdersToServerRpc(NetworkManager.Singleton.LocalClientId, reset, entityIDs, stream.ToArray());
    }

    public void AddOrResetOrders(Order order, bool reset, int[] entityIDs)
    {
        foreach (int entityID in entityIDs)
        {
            Order newOrder = (Order)order.Clone();
            Entity entity = entities[entityID];
            if (entity.type == EntityType.Unit) newOrder.AddToUnit((Unit)entity, reset); else newOrder.AddToBuilding((Building)entity, reset);
        }
    }

    public int RegisterEntity(Entity entity, int id = -1)
    {
        if (id == -1)
        {
            entities.Add(entity);
            return entities.Count - 1;
        }

        for (int i = 0; i < id - entities.Count + 1; i++)
        {
            entities.Add(null);
        }

        entities[id] = entity;
        return id;
    }

    public void Update()
    {
        foreach (Entity entity in entities)
        {
            entity.ExecuteOrders();
        }
    }
}
