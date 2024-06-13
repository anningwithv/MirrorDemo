using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyDeadState : FSMState<EnemyController>
    {
        public override void Enter(EnemyController entity)
        {
            base.Enter(entity);

        }

        public override void Execute(EnemyController entity, float dt)
        {
            base.Execute(entity, dt);
        }

        public override void Exit(EnemyController entity)
        {
            base.Exit(entity);
        }

    }
}
