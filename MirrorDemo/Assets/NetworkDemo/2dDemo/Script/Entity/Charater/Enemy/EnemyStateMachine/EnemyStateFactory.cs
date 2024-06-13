
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public enum EnemyState
    {
        None,
        Idle,
        Move,  
        Attack,
        Dead,
    }

    public class EnemyStateFactory : FSMStateFactory<EnemyController>
    {
        public EnemyStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
        }

        public void Init()
        {
            RegisterState<EnemyState>(EnemyState.None, new EnemyNoneState());
            RegisterState<EnemyState>(EnemyState.Idle, new EnemyIdleState());
            RegisterState<EnemyState>(EnemyState.Move, new EnemyMoveState());
            RegisterState<EnemyState>(EnemyState.Attack, new EnemyAttackState());
            RegisterState<EnemyState>(EnemyState.Dead, new EnemyDeadState());

        }
    }
}