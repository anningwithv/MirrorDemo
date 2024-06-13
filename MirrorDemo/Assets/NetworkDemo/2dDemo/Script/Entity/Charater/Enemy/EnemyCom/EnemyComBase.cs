using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyComBase : CharacterComponent
    {
        protected EnemyController m_EnemyController;
        protected Rigidbody2D m_Rigidbody;

        public EnemyComBase() : base()
        {
        }
    }
}