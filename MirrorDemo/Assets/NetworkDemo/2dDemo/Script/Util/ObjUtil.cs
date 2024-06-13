using GameFrame;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public static class ObjUtil
{
    //public static string EnemyAssetId;
    //public static string BulletAssetId;
    //public static string BulletTowerAssetId;
    //public static string Bullet2AssetId;
    //public static string TowerAssetId;

    public static Dictionary<string, string> AssetDic = new();

    private static string m_ObjPath = "Prefab/";

    public static void RegisterPrefabs()
    {
        //TODO: get list from table config
        List<string> prefabNameList = new()
        {
            "Bullet",
            "BulletTower",
            "Enemy",
            "Bullet2",
            "BuildingTower"
        };

        foreach (var item in prefabNameList)
        {
            AssetDic[item] = RegisterPrefab(item, 100, 10);
        }

        //BulletAssetId = RegisterPrefab("Bullet", 100, 10);
        //BulletTowerAssetId = RegisterPrefab("BulletTower", 100, 10);
        //EnemyAssetId = RegisterPrefab("Enemy", 100, 10);
        //Bullet2AssetId = RegisterPrefab("Bullet2", 100, 10);
        //TowerAssetId = RegisterPrefab("BuildingTower", 100, 10);

        //string path1 = $"{m_ObjPath}Bullet";
        //GameObject bullet = Resources.Load(path1) as GameObject;
        //NetworkClient.RegisterPrefab(bullet, SpawnHandler, UnspawnHandler);
        //var bulletAssetId = bullet.GetComponent<NetworkIdentity>().assetId;
        //GameObjectPoolMgr.S.AddPool(bulletAssetId.ToString(), bullet, 100, 10);
        //BulletAssetId = bulletAssetId.ToString();

        //string path2 = $"{m_ObjPath}BulletTower";
        //GameObject bulletTower = Resources.Load(path2) as GameObject;
        //NetworkClient.RegisterPrefab(bulletTower, SpawnHandler, UnspawnHandler);
        //var bulletTowerAssetId = bulletTower.GetComponent<NetworkIdentity>().assetId;
        //GameObjectPoolMgr.S.AddPool(bulletTowerAssetId.ToString(), bulletTower, 100, 10);
        //BulletTowerAssetId = bulletTowerAssetId.ToString();

        //string path3 = $"{m_ObjPath}Enemy";
        //GameObject enemy = Resources.Load(path3) as GameObject;
        //NetworkClient.RegisterPrefab(enemy, SpawnHandler, UnspawnHandler);
        //var enemyAssetId = enemy.GetComponent<NetworkIdentity>().assetId;
        //GameObjectPoolMgr.S.AddPool(enemyAssetId.ToString(), enemy, 100, 10);
        //EnemyAssetId = enemyAssetId.ToString();

        //string path4 = $"{m_ObjPath}Bullet2";
        //GameObject bullet2 = Resources.Load(path4) as GameObject;
        //NetworkClient.RegisterPrefab(bullet2, SpawnHandler, UnspawnHandler);
        //var bullet2AssetId = bullet2.GetComponent<NetworkIdentity>().assetId;
        //GameObjectPoolMgr.S.AddPool(bullet2AssetId.ToString(), bullet2, 100, 10);
        //Bullet2AssetId = bullet2AssetId.ToString();

        //string path5 = $"{m_ObjPath}Tower";
        //GameObject tower = Resources.Load(path5) as GameObject;
        //NetworkClient.RegisterPrefab(tower, SpawnHandler, UnspawnHandler);
        //var towerAssetId = tower.GetComponent<NetworkIdentity>().assetId;
        //GameObjectPoolMgr.S.AddPool(towerAssetId.ToString(), tower, 100, 10);
        //TowerAssetId = towerAssetId.ToString();
    }

    public static string GetAssetId(string name)
    {
        if(AssetDic.ContainsKey(name))
            return AssetDic[name];

        return string.Empty;
    }

    private static string RegisterPrefab(string path, int maxCount, int initCount)
    {
        string fullPath = $"{m_ObjPath}{path}";
        GameObject prefab = Resources.Load(fullPath) as GameObject;
        NetworkClient.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
        var assetId = prefab.GetComponent<NetworkIdentity>().assetId;
        GameObjectPoolMgr.S.AddPool(assetId.ToString(), prefab, 100, 10);
        return assetId.ToString();
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
