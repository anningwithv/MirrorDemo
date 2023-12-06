using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowedTarget
{
    Vector3 GetMoveDir();
    Transform transform { get; }
}
