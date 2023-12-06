using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletController : NetworkBehaviour
{
    private Vector3 m_MoveDir;
    private float m_Speed = 3;
    private float m_Damage = 10;

    public void SetMoveDir(Vector3 dir)
    {
        m_MoveDir = dir;
    }

    [Server]
    private void Update()
    {
        transform.position += m_MoveDir * Time.deltaTime * m_Speed;
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if(enemy != null)
        {
            enemy.OnAttacked(m_Damage);
            Destroy(gameObject);
        }
    }
}
