using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Select : MonoBehaviour
{
    // private float3 _startingPos;
    // private Camera _camera;
    // [SerializeField] private Transform spriteTransform;
    // private List<BattleObject> _selectedBattleObjects;
    //
    // public void Start()
    // {
    //     _selectedBattleObjects = new List<BattleObject>();
    //     _camera = Camera.main;
    // }
    //
    // void Update()
    // {
    //     float3 currentPos = _camera.ScreenToWorldPoint(Input.mousePosition);
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         _startingPos = _camera.ScreenToWorldPoint(Input.mousePosition);
    //         spriteTransform.gameObject.SetActive(true);
    //     }
    //
    //     if (Input.GetMouseButton(0))
    //     {
    //         
    //         Vector3 bottomLeft = new Vector3(
    //             Mathf.Min(_startingPos.x, currentPos.x),
    //             Mathf.Min(_startingPos.y, currentPos.y)
    //         );
    //         Vector3 topRight = new Vector3(
    //             Mathf.Max(_startingPos.x, currentPos.x),
    //             Mathf.Max(_startingPos.y, currentPos.y)
    //         );
    //         transform.localScale = topRight - bottomLeft;
    //         transform.position = bottomLeft;
    //     }
    //
    //     var selectedUnits = new List<BattleObject>();
    //     var selectedStructures = new List<BattleObject>();
    //     
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         spriteTransform.gameObject.SetActive(false);
    //         Collider2D[] selectedObjects = Physics2D.OverlapAreaAll(
    //             new Vector2(_startingPos.x, _startingPos.y), new Vector2(currentPos.x, currentPos.y)
    //             );
    //         foreach (var selectedObject in selectedObjects)
    //         {
    //             if (selectedObject.GetComponent<Unit>() != null)
    //             {
    //                 selectedUnits.Add(selectedObject.GetComponent<BattleObject>());
    //             }
    //
    //             if (selectedObject.GetComponent<Structure>() != null)
    //             {
    //                 selectedStructures.Add(selectedObject.GetComponent<BattleObject>());
    //             }
    //         }
    //     }
    //
    //     if (selectedUnits.Count + selectedStructures.Count > 0)
    //     {
    //         foreach (var battleObject in _selectedBattleObjects)
    //         {
    //             battleObject.SetSelected(false);
    //         }
    //
    //         _selectedBattleObjects = selectedUnits.Count > 0 ? selectedUnits : selectedStructures;
    //         
    //         foreach (var battleObject in _selectedBattleObjects)
    //         {
    //             battleObject.SetSelected(true);
    //         }
    //     }
    // }
}
