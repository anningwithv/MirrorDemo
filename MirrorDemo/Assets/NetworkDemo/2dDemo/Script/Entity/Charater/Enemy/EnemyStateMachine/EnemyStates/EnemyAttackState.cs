using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyAttackState : FSMState<EnemyController>
    {
        private float m_AtkRange = 2;
        private float m_AtkInterval = 2;
        private float m_AtkTimer;
        private bool m_IsAttacking;

        public override void Enter(EnemyController entity)
        {
            base.Enter(entity);

            m_AtkTimer = m_AtkInterval;
            m_IsAttacking = false;
        }

        public override void Execute(EnemyController entity, float dt)
        {
            base.Execute(entity, dt);

            var target = entity.FindTargetCom.Target;
            if (target == null || !target.IsAlive())
            {
                entity.SetState( EnemyState.Idle);
                return;
            }

            float distance = Vector3.Distance(target.transform.position, entity.transform.position);
            if (distance > m_AtkRange + 0.5f)
            {
                entity.SetState(EnemyState.Move);
                return;
            }

            if (!m_IsAttacking)
            {
                m_AtkTimer += dt;
                if (m_AtkTimer >= m_AtkInterval)
                {
                    m_AtkTimer = 0;
                    Attack(entity, target);
                }
            }

            entity.Rgb.velocity = Vector2.zero;
        }

        public override void Exit(EnemyController entity)
        {
            base.Exit(entity);
        }

        private void Attack(EnemyController entity, CharacterController target)
        {
            if (target == null)
                return;

            m_IsAttacking = true;

            Vector3 dir = target.transform.position - entity.transform.position;
            entity.FaceToDir(dir);
            //entity.StopMove();
            entity.AnimCom.PlayAttackAnim(() =>
            {
                if (target == null)
                    return;

                float distance = Vector3.Distance(entity.transform.position, target.transform.position);
                if (distance < m_AtkRange)
                {
                    //target.OnAttacked(entity.Controller, entity.Controller.Damage);
                }

            }, () =>
            {
                m_IsAttacking = false;
            });
        }
    }
}
