using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameFrame;

/// <summary>
/// Enemy is Server obj
/// </summary>
public class EnemyController : NetworkBehaviour
{
    private PlayerController m_Target;
    private float m_MoveSpeed = 1f;
    private Rigidbody2D m_Rigidbody;
    private float m_CurHp;

    public override void OnStartServer()
    {
        base.OnStartServer();

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_CurHp = 50;
    }

    [Server]
    private void Update()
    {
        FindTaget();
        MoveToTarget();
    }

    #region Server
    [Server]
    public void OnAttacked(float damage)
    {
        m_CurHp -= damage;
        if (m_CurHp < 0)
        {
            //Destroy(gameObject);
            GameObjectPoolMgr.S.Recycle(gameObject);
        }
    }

    [Server]
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

    [Server]
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
