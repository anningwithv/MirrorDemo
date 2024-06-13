using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public static class ObjUtil
{
    public static string EnemyAssetId;

    private static string m_ObjPath = "Prefab/";
    public static void RegisterPrefabs()
    {
        string path1 = $"{m_ObjPath}Bullet";
        GameObject bullet = Resources.Load(path1) as GameObject;
        NetworkClient.RegisterPrefab(bullet, SpawnHandler, UnspawnHandler);
        var bulletAssetId = bullet.GetComponent<NetworkIdentity>().assetId;
        GameObjectPoolMgr.S.AddPool(bulletAssetId.ToString(), bullet, 100, 10);

        string path2 = $"{m_ObjPath}BulletTower";
        GameObject bulletTower = Resources.Load(path2) as GameObject;
        NetworkClient.RegisterPrefab(bulletTower, SpawnHandler, UnspawnHandler);
        var bulletTowerAssetId = bulletTower.GetComponent<NetworkIdentity>().assetId;
        GameObjectPoolMgr.S.AddPool(bulletTowerAssetId.ToString(), bulletTower, 100, 10);

        string path3 = $"{m_ObjPath}Enemy";
        GameObject enemy = Resources.Load(path3) as GameObject;
        NetworkClient.RegisterPrefab(enemy, SpawnHandler, UnspawnHandler);
        var enemyAssetId = enemy.GetComponent<NetworkIdentity>().assetId;
        GameObjectPoolMgr.S.AddPool(enemyAssetId.ToString(), enemy, 100, 10);
        EnemyAssetId = enemyAssetId.ToString();

        string path4 = $"{m_ObjPath}Bullet2";
        GameObject bullet2 = Resources.Load(path4) as GameObject;
        NetworkClient.RegisterPrefab(bullet2, SpawnHandler, UnspawnHandler);
        var bullet2AssetId = bullet2.GetComponent<NetworkIdentity>().assetId;
        GameObjectPoolMgr.S.AddPool(bullet2AssetId.ToString(), bullet2, 100, 10);
    }

    private static GameObject SpawnHandler(SpawnMessage msg)
    {
        GameObject obj = GameObjectPoolMgr.S.Allocate(msg.assetId.ToString());
        obj.transform.position = msg.position;
        obj.transform.rotation = msg.rotation;
        obj.transform.parent = ServerGameMgr.Instance.EntityRoot;
        return obj;
    }

    private static void UnspawnHandler(GameObject spawned)
    {
        GameObjectPoolMgr.S.Recycle(spawned);
    }
}
