using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : NetworkBehaviour
{
    public override void OnStartServer()
    {
        OnStart();
    }

    [Server]
    protected virtual void OnStart()
    { 
    }

    [Server]
    protected void Update()
    {
        OnUpdate();
    }

    [Server]
    protected virtual void OnUpdate()
    {

    }
}
