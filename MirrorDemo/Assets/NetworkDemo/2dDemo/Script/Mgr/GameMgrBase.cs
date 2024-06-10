using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameMgrBase<T> : NetworkBehaviour
{
    //public GameObject EnemyPrefab;

    public static T Instance;

    protected Dictionary<int, ISubMgr> m_SubMgrDic = new();

    #region Tool Func

    [Server]
    public T AddSubMgr<T>() where T : ISubMgr, new()
    {
        T mgr = new T();
        m_SubMgrDic.Add(typeof(T).GetHashCode(), mgr);
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
