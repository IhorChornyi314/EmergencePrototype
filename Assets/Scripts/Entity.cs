using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public enum EntityType
{
    Building,
    Unit
}

public abstract class Entity : NetworkBehaviour
{
    public int team = -1;
    public EntityType type;
    public EntityData data;
    

    public Vector2 Position
    {
        get => _pos;
        set
        {
            _pos = value;
            transform.position = value;
        }
    }
    public Queue<Order> orders = new Queue<Order>();
    private Vector2 _pos;


    public void Awake()
    {
        string prefabName = name.Split('(')[0];
        data = Resources.Load<EntityData>(string.Format("EntityData/{0}Data", prefabName));
    }


    public void AddOrResetOrder(Order order, bool reset)
    {
        if (reset)
        {
            orders.Clear();
        }
        orders.Enqueue(order);
    }
    
    public void SetSelected(bool selected)
    {
        GetComponent<SpriteRenderer>().color = selected ? Color.green : Color.white;
    }

    public void ExecuteOrders()
    {
        if (orders.Count > 0)
            orders.Peek().Execute(this);
    }
}
