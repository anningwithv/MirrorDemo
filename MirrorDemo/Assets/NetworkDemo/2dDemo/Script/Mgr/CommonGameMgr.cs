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
        NetworkMgr.OnGameBegin += OnGameBegin;
        EntityRoot = transform.Find("CommonEntityRoot");
        m_IsGameBegin = false;

        InitSubMgrs();
    }

    private void InitSubMgrs()
    {
        //CommonObjMgr commonObjMgr = AddSubMgr<CommonObjMgr>();
        //commonObjMgr.Init();

        CommonObjMgr commonObjMgr = new CommonObjMgr();
        commonObjMgr.Init();

        int key = typeof(CommonObjMgr).GetHashCode();
        if (!m_SubMgrDic.ContainsKey(key))
            m_SubMgrDic.Add(key, commonObjMgr);
    }

    private void OnGameBegin()
    {
        foreach (var subMgr in m_SubMgrDic.Values)
        {
            subMgr.OnGameBegin();
        }
        m_IsGameBegin = true;
    }
    #endregion

    #region Update

    private void Update()
    {
        if (!m_IsGameBegin)
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
