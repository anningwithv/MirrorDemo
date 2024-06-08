using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBase : NetworkBehaviour
{
    public GameObject FollowerPrefab;

    protected PlayerWeaponController m_WeaponController;
    protected EnemyController m_AtkTarget;

    protected virtual void Awake()
    {
        m_WeaponController = GetComponentInChildren<PlayerWeaponController>();

    }

    public virtual Vector3 GetMoveDir()
    {
        return Vector3.zero;
    }

    [Client]
    protected void SearchTarget()
    {
        if (m_AtkTarget != null)
            return;

        var enemies = FindObjectsOfType<EnemyController>();
        if (enemies.Length > 0)
        {
            m_AtkTarget = enemies[0];
        }
    }

    protected float m_LastAtkTime;
    [Client]
    protected void Fire()
    {
        if (m_AtkTarget == null) return;

        if (Time.time - m_LastAtkTime > 3)
        {
            m_LastAtkTime = Time.time;

            Vector3 fireDir = m_AtkTarget.transform.position - transform.position;
            fireDir.Normalize();

            CmdFire(fireDir, transform.position);
        }
    }

    [Command]
    protected void CmdFire(Vector3 dir, Vector3 position)
    {
        m_WeaponController.Fire(dir, position);
        //GameObject go = GameObject.Instantiate(Bullet, position, Quaternion.identity);
        //NetworkServer.Spawn(go);

        //go.GetComponent<BulletController>().SetMoveDir(dir);
    }

}
