using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public abstract class CharacterComponent
    {
        protected ICharacterComOwner m_Owner;

        //public CharacterComponent(ICharacterComOwner owner)
        //{
        //    m_Owner = owner;
        //}

        public CharacterComponent()
        { }

        public void SetOwner(ICharacterComOwner owner)
        { m_Owner = owner; }    

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate(float delta)
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnRecycled()
        {
        }
    }
}