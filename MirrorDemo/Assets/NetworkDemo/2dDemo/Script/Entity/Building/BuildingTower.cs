using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTower : BuildingBase
{
    public GameObject BulletPrefab;
    protected float m_LastAtkTime;
    protected EnemyController m_AtkTarget;
    [Server]
    protected override void OnStart()
    {
        base.OnStart();
    }

    [Server]
    protected override void OnUpdate()
    {
        base.OnUpdate();

        SearchTarget();

        if (Time.time - m_LastAtkTime > 5)
        {
            m_LastAtkTime = Time.time;

            Vector3 fireDir = m_AtkTarget.transform.position - transform.position;
            fireDir.Normalize();

            Fire(fireDir, transform.position);
        }
    }

    [Server]
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

    [Server]
    protected void Fire(Vector3 dir, Vector3 position)
    {
        //GameObject go = GameObject.Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        //GameObject go = GameObjectPoolMgr.S.Allocate(ObjUtil.BulletTowerAssetId);
        string assetId = ObjUtil.GetAssetId("BulletTower");
        GameObject go = GameObjectPoolMgr.S.Allocate(assetId);
        go.transform.parent = ServerGameMgr.Instance.EntityRoot;
        go.transform.position = transform.position;

        go.GetComponent<BulletController>().SetMoveDir(dir);

        NetworkServer.Spawn(go);

    }
}
