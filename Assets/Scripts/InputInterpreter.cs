using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInterpreter : MonoBehaviour
{
    private bool _shiftDown;
    public Dictionary<string, IInputActionCollection2> ActionMaps = new Dictionary<string, IInputActionCollection2>();
    public static InputInterpreter Instance;
    public void Awake()
    {
        Instance = this;
        UnitControl unitControl = new UnitControl();
        unitControl.BattleMap.RightClick.performed += ProcessRightClick;
        ActionMaps.Add("UnitControl", unitControl);
    }

    public void ProcessShift(InputAction.CallbackContext context) => _shiftDown = context.started || context.performed;
    
    public void ProcessRightClick(InputAction.CallbackContext context)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        EntityManager.Instance.SetTarget(point, !_shiftDown);
    }
}
