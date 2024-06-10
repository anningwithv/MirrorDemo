using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonObjMgr : CommonSubMgr
{
    private string m_ObjPath = "Prefab/";

    public override void Init()
    {
        base.Init();
        //m_ServerGameMgr.NetworkMgr.spawnPrefabs.Add(bullet);
        //InitPools();
    }

    public override void OnGameBegin()
    {
        base.OnGameBegin();

       
    }

    public override void Tick()
    {
    }

    public override void Clear()
    {
    }

    private void InitPools()
    {
        string path1 = $"{m_ObjPath}Bullet";
        GameObject bullet = Resources.Load(path1) as GameObject;
        //NetworkClient.RegisterPrefab(bullet, SpawnHandler, UnspawnHandler);
        //NetworkClient.RegisterPrefab(bullet);
        GameObjectPoolMgr.S.AddPool(bullet.name, bullet, 100, 10);

        string path2 = $"{m_ObjPath}BulletTower";
        GameObject bulletTower = Resources.Load(path2) as GameObject;
        //NetworkClient.RegisterPrefab(bulletTower);
        GameObjectPoolMgr.S.AddPool(bulletTower.name, bulletTower, 100, 10);

        string path3 = $"{m_ObjPath}Enemy";
        GameObject enemy = Resources.Load(path3) as GameObject;
        //NetworkClient.RegisterPrefab(enemy);
        GameObjectPoolMgr.S.AddPool(enemy.name, enemy, 100, 10);

        string path4 = $"{m_ObjPath}Bullet2";
        GameObject bullet2 = Resources.Load(path4) as GameObject;
        //NetworkClient.RegisterPrefab(bullet2);
        GameObjectPoolMgr.S.AddPool(bullet2.name, bullet2, 100, 10);
    }
    //private GameObject SpawnHandler(SpawnMessage msg)
    //{
    //    return GetFromPool(msg.position, msg.rotation);
    //}

    //private void UnspawnHandler(GameObject spawned)
    //{
    //    PutBackInPool(spawned);
    //}
}
