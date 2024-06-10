using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubMgr
{
    void Init();

    void OnGameBegin();
    void Tick();
    void Clear();
}
