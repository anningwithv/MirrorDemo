using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Game mgr only run in both server and client
/// </summary>
public class CommonGameMgr : GameMgrBase<CommonGameMgr>
{
    public NetworkMgr NetworkMgr { get; private set; }
    public Transform EntityRoot { get; private set; }


    #region Init

    public override void OnStartServer()
    {
        base.OnStartServer();

        Init();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Init();
    }

    private void Init()
    {
        Instance = this;

        NetworkMgr = FindObjectOfType<NetworkMgr>();
        NetworkMgr.OnGameBegin = OnGameBegin;
        EntityRoot = transform.Find("CommonEntityRoot");

        InitSubMgrs();
    }

    private void InitSubMgrs()
    {
        CommonObjMgr commonObjMgr = AddSubMgr<CommonObjMgr>();
        commonObjMgr.Init();
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
