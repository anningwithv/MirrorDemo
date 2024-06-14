using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameFrame;
using ProjectX.Logic;

/// <summary>
/// Enemy is Server obj
/// </summary>
public class EnemyController : CharacterController
{
    private PlayerController m_Target;
    private float m_MoveSpeed = 1f;

    protected EnemyStateMachine m_StateMachine;
    protected EnemyState m_CurState;

    public EnemyFindTargetCom FindTargetCom { get; private set; }
    public EnemyAnimCom AnimCom { get; private set; }

    #region Server

    #region FrameFunc
    protected override void Awake()
    {
        base.Awake();

        //Init coms
        FindTargetCom = RegisterCom<EnemyFindTargetCom>();
        AnimCom = RegisterCom<EnemyAnimCom>();
        AnimCom.Init(m_SpineAnim);
        HealthCom.InitHealth(50);

        //Init params
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        //Init state machine
        if (m_StateMachine == null)
            m_StateMachine = new EnemyStateMachine(this);
        SetState(EnemyState.Idle);
    }

    [Server]
    protected override void Update()
    {
        base.Update();

        UpdateStateMachine(Time.deltaTime);

        MoveToTarget();
    }
    #endregion

    #region Public 
    [Server]
    public void SetState(EnemyState enemyState)
    {
        if (m_CurState != enemyState)
        {
            m_CurState = enemyState;
            m_StateMachine.SetCurrentState(m_StateMachine.stateFactory.GetState(m_CurState));
        }
    }

    [Server]
    public void OnAttacked(float damage)
    {
        HealthCom.AddHealth(-damage);
        if (!HealthCom.IsAlive())
        {
            GameObjectPoolMgr.S.Recycle(gameObject);
        }
    }
    #endregion

    #region Private

    [Server]
    private void MoveToTarget()
    {
        if(m_Target == null)
        {
            return;
        }

        var dir = m_Target.transform.position - transform.position;
        dir = dir.normalized;
        dir.z = 0;

        Rgb.velocity = dir * m_MoveSpeed;
    }

    [Server]
    private void UpdateStateMachine(float dt)
    {
        m_StateMachine.UpdateState(Time.deltaTime);
    }
    #endregion

    #endregion
}
