using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServerSubMgr
{
    protected ServerGameMgr m_ServerGameMgr;

    public virtual void Init() {
        m_ServerGameMgr = ServerGameMgr.Instance;
    }

    public virtual void OnGameBegin()
    { 
    }
    public abstract void Tick();
    public abstract void Clear();
}
