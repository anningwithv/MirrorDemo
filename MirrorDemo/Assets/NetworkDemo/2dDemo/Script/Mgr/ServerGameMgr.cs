using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Game mgr only run in server
/// </summary>
public class ServerGameMgr : GameMgrBase<ServerGameMgr>
{
    public NetworkMgr NetworkMgr { get; private set; }
    public Transform EntityRoot { get; private set; }


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
        NetworkMgr.OnGameBegin = OnGameBegin;
        EntityRoot = transform.Find("ServerEntityRoot");

        InitSubMgrs();
    }

    [Server]
    private void InitSubMgrs()
    {
        ServerEnemyMgr serverEnemyMgr = AddSubMgr<ServerEnemyMgr>();
        serverEnemyMgr.Init();

        CommonObjMgr serverObjMgr = AddSubMgr<CommonObjMgr>();
        serverObjMgr.Init();
    }

    private void OnGameBegin()
    {
        foreach (var subMgr in m_SubMgrDic.Values)
        {
            subMgr.OnGameBegin();
        }
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

    #endregion

}