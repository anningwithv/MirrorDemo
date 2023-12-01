using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelController : NetworkBehaviour
{
    public GameObject EnemyPrefab;

    public static LevelController Instance;

    private List<Transform> m_FollowList;

    private void Awake()
    {
        Instance = this;
        m_FollowList = new List<Transform>();
    }

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Server]
    private void Update()
    {
        if (NetworkMgr.singleton.IsGameBegin)
        {
            SpawnEnemies();
        }
    }

    private float m_LastSpawnTime;
    private void SpawnEnemies()
    {
        if (Time.time - m_LastSpawnTime > 5)
        {
            m_LastSpawnTime = Time.time;

            int count = 2;
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0);
                GameObject go = GameObject.Instantiate(EnemyPrefab);
                go.transform.position = pos;
                NetworkServer.Spawn(go);
            }
        }
    }

    #endregion

    #region Client

    public void AddFollower(Transform trans)
    {
        m_FollowList.Add(trans);
    }

    #endregion
}
