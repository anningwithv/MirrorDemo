using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemyMgr : ServerSubMgr
{
    private float m_LastSpawnTime;

    public override void Init()
    {
        base.Init();
    }

    public override void Tick()
    {
        SpawnEnemies();
    }

    public override void Clear()
    {
    }

    private void SpawnEnemies()
    {
        if (Time.time - m_LastSpawnTime > 5)
        {
            m_LastSpawnTime = Time.time;

            int count = 2;
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                GameObject go = GameObjectPoolMgr.S.Allocate("Enemy") ;
                go.transform.parent = m_ServerGameMgr.EntityRoot;
                go.transform.position = pos;
                go.SetActive(true);
                //NetworkServer.Spawn(go);
            }
        }
    }

}
