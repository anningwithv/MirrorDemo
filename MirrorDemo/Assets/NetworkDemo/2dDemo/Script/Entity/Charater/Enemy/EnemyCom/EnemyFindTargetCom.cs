using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyFindTargetCom : EnemyComBase
    {
        public CharacterController Target { get; set; }

        private float m_Timer;
        private float m_FindTargetInterval = 2;
        private List<CharacterController> m_PotentialTargetList = new List<CharacterController>();

        public EnemyFindTargetCom() : base()
        {
            m_Timer = m_FindTargetInterval;
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

            m_Timer += delta;
            if (m_Timer > m_FindTargetInterval)
            {
                m_Timer = 0;

                RefreshTarget();
              
            }

        }

        List<CharacterController> tmpList = new List<CharacterController>();
        public void RefreshTarget()
        {
            var normalHeroes = GameObject.FindObjectsOfType<PlayerController>();
            tmpList.Clear();
            tmpList.AddRange(normalHeroes);

            Target = FindTarget(tmpList);
        }

        private CharacterController FindTarget(List<CharacterController> list)
        {
            if (list == null || list.Count == 0)
                return null;

            //float distance = float.MaxValue;
            //CharacterController character = null;
            for (int i = 0; i < list.Count; i++)
            {
                float curDistance = Vector3.Distance(m_Owner.Transform.position, list[i].transform.position);

                float distancePriority = Mathf.Clamp(10 - curDistance, 0, 10);

                //list[i].SearchPriority = distancePriority + list[i].HateValue;

            }

            if (list.Count > 0)
            {
                //list.Sort((a, b) => a.SearchPriority > b.SearchPriority ? -1 : 1);

                return list[0];
            }

            return null;
        }
    }

}