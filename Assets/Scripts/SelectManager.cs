using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class 
SelectManager : MonoBehaviour
{
    public List<int> selectedEntities = new List<int>();
    public static SelectManager Instance;

    public void Awake() => Instance = this;

    public void Flush()
    {
        foreach (int selectedEntity in selectedEntities)
        {
            EntityManager.Instance.entities[selectedEntity].SetSelected(false);
        }
        selectedEntities = new List<int>();
        InputInterpreter.Instance.ActionMaps["UnitControl"].Disable();
    }

    public void AddSelected(Entity entity)
    {
        entity.SetSelected(true);
        selectedEntities.Add(EntityManager.Instance.GetEntityID(entity));
        InputInterpreter.Instance.ActionMaps["UnitControl"].Enable();
    }
    
    public void AddSelected(int entityID)
    {
        selectedEntities.Add(entityID);
    }

    public void SelectGroup(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            int groupIndex = int.Parse(context.action.name);
            Instance.Flush();
            EntityManager.Instance.groups[groupIndex].Select();
        }
    }
}
