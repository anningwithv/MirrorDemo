using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyIdleState : FSMState<EnemyController>
    {
        public override void Enter(EnemyController entity)
        {
            base.Enter(entity);
        }

        public override void Execute(EnemyController entity, float dt)
        {
            base.Execute(entity, dt);

            if (entity.FindTargetCom.Target != null)
            {
                entity.SetState(EnemyState.Move);
            }
        }

        public override void Exit(EnemyController entity)
        {
            base.Exit(entity);
        }

    }
}
