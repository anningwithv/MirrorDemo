using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyComBase : CharacterComponent
    {
        protected EnemyController m_Controller;

        public EnemyComBase() : base()
        {
        }

        public override void SetOwner(ICharacterComOwner owner)
        {
            base.SetOwner(owner);

            m_Controller = (EnemyController)owner;
        }
    }
}