using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace ProjectX.Logic
{
    public class HealthCom : CharacterComponent
    {
        private CharacterHpBar m_HpBar;
        private float m_Health;
        private float m_MaxHealth;

        public float MaxHealth { get => m_MaxHealth; }

        //public HealthCom(ICharacterComOwner owner) : base(owner)
        //{
        //    m_Health = 100;
        //    m_MaxHealth = 100;
        //}

        public HealthCom() {}

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

        }

        public void InitHealth(float health)
        {
            m_Health = m_MaxHealth = health;

            m_Owner.RefreshHpPercent(100);
        }

        //public void SetHealth(float health)
        //{
        //    m_Health = health;
        //}

        public float GetHealth()
        {
            return m_Health;
        }

        public float GetHealthPercent()
        {
            return m_Health / m_MaxHealth * 100;
        }

        public bool IsAlive()
        {
            return m_Health > 0;
        }

        //public void HandleDamage(float damage)
        //{
        //    m_Health -= damage;
        //    m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
        //    //Log.i("Handle damage: " + damage + " health: " + m_Health + " name: " + m_Owner.Transform.name);
        //}

        public void AddHealth(float delta)
        {
            m_Health += delta;
            m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);

            float percent = GetHealthPercent();
            RefreshHpBar(percent);

            m_Owner.RefreshHpPercent(percent);
        }

        public void RefrehMaxHp(float max)
        {
            m_MaxHealth = max;
            m_Health = Mathf.Min(m_Health, m_MaxHealth);
        }

        public void RefreshHpBar(float percent)
        {
            if (m_HpBar == null)
            {
                m_HpBar = m_Owner.Transform.GetComponentInChildren<CharacterHpBar>();
            }

            if (m_HpBar != null)
            {
                m_HpBar.Refresh(percent);
            }
        }
    }
}