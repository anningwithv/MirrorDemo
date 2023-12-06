using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowerController : CharacterBase
{
    private IFollowedTarget m_Target;
    private Vector3 m_LastPosition;
    private Vector3 m_MoveDir;

    public override void OnStartClient()
    {
        base.OnStartClient();

        m_LastPosition = transform.position;
        m_MoveDir = Vector3.zero;
    }

    [Client]
    private void Update()
    {
        if(isOwned)
        {
            if (m_Target == null)
            {
                m_Target = LevelMgr.Instance.GetLastTarget();
            }

            transform.position = m_Target.transform.position - m_Target.GetMoveDir()*0.7f;
            m_MoveDir = transform.position - m_LastPosition;
            m_MoveDir.Normalize();
            m_LastPosition = transform.position;

            SearchTarget();
            Fire();
        }
    }

    public override Vector3 GetMoveDir()
    {
        return m_MoveDir;
    }

}
