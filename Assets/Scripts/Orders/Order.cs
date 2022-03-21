using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum OrderResult
{
    Continue,
    Finish
}

[Serializable]
public abstract class Order : ICloneable
{
    public abstract OrderResult Execute(Entity self);
    
    public abstract void AddToBuilding(Building building, bool reset);
    
    public abstract void AddToUnit(Unit unit, bool reset);

    public abstract void Serialize();
    public abstract void Deserialize();

    public object Clone()
    {
        return MemberwiseClone();
    }
}
