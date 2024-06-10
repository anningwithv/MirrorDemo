using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameMgrBase<K> : NetworkBehaviour
{
    //public GameObject EnemyPrefab;

    public static K Instance;

    protected Dictionary<int, ISubMgr> m_SubMgrDic = new();
    protected bool m_IsGameBegin;
    #region Tool Func

    [Server]
    public T AddSubMgr<T>() where T : ISubMgr, new()
    {
        T mgr = new T();
        int key = typeof(T).GetHashCode();
        if (!m_SubMgrDic.ContainsKey(key))
            m_SubMgrDic.Add(key, mgr);
        return mgr;
    }

    [Server]
    public T GetSubMgr<T>() where T : ISubMgr, new()
    {
        int key = typeof(T).GetHashCode();
        if(m_SubMgrDic.ContainsKey(key))
            return (T)m_SubMgrDic[key];
        return default;
    }

    #endregion

}
