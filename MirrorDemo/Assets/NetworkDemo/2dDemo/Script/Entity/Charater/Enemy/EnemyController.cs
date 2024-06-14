using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameFrame;
using ProjectX.Logic;

/// <summary>
/// Enemy logic run in Server and sync value to client
/// </summary>
public class EnemyController : CharacterController
{
    protected EnemyStateMachine m_StateMachine;
    protected EnemyState m_CurState;

    public EnemyFindTargetCom FindTargetCom { get; private set; }
    public EnemyAnimCom AnimCom { get; private set; }
    
    #region SyncVar
    [SyncVar(hook = nameof(OnAnimStateChanged))]
    public EnemyAnimState AnimState;

    [SyncVar(hook = nameof(OnHpChanged))]
    public float HpPercent;
    #endregion

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

        FloatingTextUtil.CreateText($"<b>{damage}</b>", transform.position + new Vector3(0, 1, 0), Color.red);

        if (!HealthCom.IsAlive())
        {
            GameObjectPoolMgr.S.Recycle(gameObject);
            NetworkServer.UnSpawn(gameObject);
        }
    }

    public override void RefreshHpPercent(float percent)
    {
        base.RefreshHpPercent(percent);

        HpPercent = percent;
    }
    #endregion

    #region Private

    [Server]
    private void UpdateStateMachine(float dt)
    {
        m_StateMachine.UpdateState(Time.deltaTime);
    }
    #endregion

    #endregion

    #region Hook func
    /// <summary>
    /// Run in client, sync animation
    /// </summary>
    private void OnAnimStateChanged(EnemyAnimState oldState, EnemyAnimState newState)
    {
        Log.i($"On Anim state changed: {newState}");
        switch (newState)
        {
            case EnemyAnimState.Idle:
                AnimCom.PlayIdleAnim();
                break;
            case EnemyAnimState.Move:
                AnimCom.PlayMoveAnim();
                break;
            case EnemyAnimState.Attack:
                AnimCom.PlayAttackAnim(null, null);
                break;
        }
    }

    private void OnHpChanged(float oldHp, float newHp)
    {
        HealthCom.RefreshHpBar(newHp);
    }

    #endregion
}
