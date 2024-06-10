using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Game mgr only run in server
/// </summary>
public class ServerGameMgr : NetworkBehaviour
{
    //public GameObject EnemyPrefab;

    public static ServerGameMgr Instance;

    public NetworkMgr NetworkMgr { get; private set; }
    public Transform EntityRoot { get; private set; }

    private Dictionary<int, ServerSubMgr> m_SubMgrDic = new();


    #region Init
    public override void OnStartServer()
    {
        base.OnStartServer();

        Instance = this;

        Init();
    }

    [Server]
    private void Init()
    {
        NetworkMgr = FindObjectOfType<NetworkMgr>();
        EntityRoot = transform.Find("EntityRoot");

        InitSubMgrs();
    }

    [Server]
    private void InitSubMgrs()
    {
        ServerEnemyMgr serverEnemyMgr = AddSubMgr<ServerEnemyMgr>();
        serverEnemyMgr.Init();

        ServerObjMgr serverObjMgr = AddSubMgr<ServerObjMgr>();
        serverObjMgr.Init();
    }


    #endregion

    #region Update
    [Server]
    private void Update()
    {
        if (!NetworkMgr.singleton.IsGameBegin)
            return;

        foreach (var subMgr in m_SubMgrDic.Values)
        {
            subMgr.Tick();
        }
    }
    #endregion

    #region Tool Func

    [Server]
    public T AddSubMgr<T>() where T : ServerSubMgr, new()
    {
        T mgr = new T();
        m_SubMgrDic.Add(typeof(T).GetHashCode(), mgr);
        return mgr;
    }

    [Server]
    public T GetSubMgr<T>() where T : ServerSubMgr, new()
    {
        int key = typeof(T).GetHashCode();
        if(m_SubMgrDic.ContainsKey(key))
            return (T)m_SubMgrDic[key];
        return null;
    }

    #endregion

}
