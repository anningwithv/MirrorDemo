using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonSubMgr : ISubMgr
{
    protected ServerGameMgr m_GameMgr;

    public virtual void Init() {
        m_GameMgr = ServerGameMgr.Instance;
    }

    public virtual void OnGameBegin()
    { 
    }
    public abstract void Tick();
    public abstract void Clear();
}
