using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroup
{
    private List<Entity> _entities = new List<Entity>();

    public EntityGroup(List<Entity> entities) => _entities = entities;
    public void Select()
    {
        foreach (Entity entity in _entities)
        {
            SelectManager.Instance.AddSelected(entity);
        }
    }

    public void AddEntities(List<Entity> entities)
    {
        _entities.AddRange(entities);
    }
}
