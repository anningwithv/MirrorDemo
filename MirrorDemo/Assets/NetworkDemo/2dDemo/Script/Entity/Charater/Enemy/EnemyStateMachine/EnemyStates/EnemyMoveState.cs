using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyMoveState : FSMState<EnemyController>
    {
        public EnemyController Controller { get => m_Controller; }
        private EnemyController m_Controller;
        private float m_Timer;
        private float m_RefrehInterval = 1;
        private float m_AtkRange = 2;

        public override void Enter(EnemyController entity)
        {
            base.Enter(entity);

            m_Controller = entity;
            m_Timer = m_RefrehInterval;

            //m_Controller.EnemyAnimCom.PlayMoveAnim();
        }

        public override void Execute(EnemyController entity, float dt)
        {
            base.Execute(entity, dt);

            var target = entity.FindTargetCom.Target;
            if (target == null || !target.IsAlive())
            {
                entity.SetState(EnemyState.Idle);
                return;
            }

            float distanceToTarget = Vector3.Distance(entity.transform.position, target.transform.position);
            if (distanceToTarget < m_AtkRange)
            {
                entity.SetState(EnemyState.Attack);
            }
            else
            {
                entity.MoveToPos(target.transform.position);
            }
        }

        public override void Exit(EnemyController entity)
        {
            base.Exit(entity);
        }
    }
}
