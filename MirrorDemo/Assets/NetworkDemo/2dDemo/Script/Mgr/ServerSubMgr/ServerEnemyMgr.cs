using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEnemyMgr : ServerSubMgr
{
    private float m_LastSpawnTime;
    private bool m_StartSpawnEnemy;

    public override void Init()
    {
        base.Init();

        m_StartSpawnEnemy = false;
    }

    public override void OnGameBegin()
    {
        base.OnGameBegin();

        m_LastSpawnTime = Time.time;
        m_StartSpawnEnemy = true;
    }

    public override void Tick()
    {
        if (!m_StartSpawnEnemy)
            return;

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
                //GameObject go = GameObjectPoolMgr.S.Allocate("Enemy") ;

                string path3 = $"Prefab/Enemy";
                GameObject enemy = Resources.Load(path3) as GameObject;
                GameObject go = GameObject.Instantiate(enemy) as GameObject;
                //go.transform.parent = m_ServerGameMgr.EntityRoot;
                go.transform.position = pos;
                go.SetActive(true);
                NetworkServer.Spawn(go);
            }
        }
    }

}
