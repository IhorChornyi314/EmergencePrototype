using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransformTest : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<Vector3> truePosition = new NetworkVariable<Vector3>();
    [SerializeField]
    private float cooldown = 1;

    public void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            truePosition.Value = transform.position;
        }
    }

    [ServerRpc]
    public void SubmitNewPositionServerRpc(Vector3 position)
    {
        truePosition.Value = position;
        transform.position = position;
        cooldown -= Time.deltaTime;
        Debug.Log("!");
        if (cooldown <= 0)
        {
            Debug.Log("!!");
            SyncPositionClientRpc();
            cooldown = 1;
        }
    }

    [ClientRpc]
    public void SyncPositionClientRpc()
    {
        transform.position = truePosition.Value;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 deltaMove = context.action.ReadValue<Vector2>();
        Debug.Log(deltaMove);
        transform.position = transform.position + deltaMove * 5;
        SubmitNewPositionServerRpc(truePosition.Value + deltaMove * 3);
    }

    public void MoveCustom()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            truePosition.Value = truePosition.Value - new Vector3(Time.deltaTime * 3, 0);
        }
        transform.position = truePosition.Value;
    }

    public void MovePreMade()
    {
        transform.position = transform.position - new Vector3(Time.deltaTime * 3, 0);
    }

    public void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            MoveCustom();
        }
    }
}
