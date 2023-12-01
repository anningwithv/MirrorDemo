using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyController : NetworkBehaviour
{
    private PlayerController m_Target;
    private float m_MoveSpeed = 1f;
    private Rigidbody2D m_Rigidbody;

    public override void OnStartServer()
    {
        base.OnStartServer();

        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    [ServerCallback]
    private void Update()
    {
        FindTaget();
        MoveToTarget();
    }

    #region Server
    private void FindTaget()
    {
        if (m_Target == null)
        {
            var targets = FindObjectsOfType<PlayerController>();
            if (targets != null && targets.Length > 0)
            { 
                m_Target = targets[0];
            }
        }
    }

    private void MoveToTarget()
    {
        if(m_Target == null)
        {
            return;
        }

        var dir = m_Target.transform.position - transform.position;
        dir = dir.normalized;
        dir.z = 0;

        m_Rigidbody.velocity = dir * m_MoveSpeed;
    }
    #endregion
}
