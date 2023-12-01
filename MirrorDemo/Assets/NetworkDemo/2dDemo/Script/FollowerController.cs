using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowerController : NetworkBehaviour
{
    private PlayerController m_PlayerControl;

    public void Init(PlayerController control)
    {
        m_PlayerControl = control;
    }

    private void Update()
    {
        if(isOwned)
        {
            transform.position = m_PlayerControl.transform.position + new Vector3(1, 0, 0);
        }
    }
}
