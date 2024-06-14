using UnityEngine;
using Spine.Unity;
using Spine;
using System.Text;
using System;
using Unity.Mathematics;
using Mirror;
using GameFrame;

namespace ProjectX.Logic
{
    public enum EnemyAnimState
    {
        Paused,
        Resumed,
        Idle,
        Move,
        Attack,
        Dead,
    }
    public class EnemyAnimCom : EnemyComBase
    {
        private float m_CheckDirInterval = 0.1f;
        private float m_CheckDirTimer = 0.1f;

        private SkeletonAnimation m_SpineAnim;

        private string m_IdleAnimName = "Idle";
        private string m_MoveAnimName = "Move";
        private string m_AnimPrefix = "";
        private string m_LastPlayAnimName;

        protected Animator m_Animator;

        public SkeletonAnimation SpineAnim { get => m_SpineAnim; }


        public EnemyAnimCom() : base()
        {

        }

        public void Init(SkeletonAnimation spine)
        {
            m_SpineAnim = spine;
        }

        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
        }

        public void OnDead()
        {
            if (m_SpineAnim != null)
            {
                PlayDeadAnim();
            }
        }

        public void OnAttacked()
        {
        }

        //private StringBuilder m_AnimNameBuilder = new StringBuilder();
        private TrackEntry PlayAnim(string animName, bool loop, float speed = 1)
        {
            if (m_SpineAnim == null)
                return null;

            m_LastPlayAnimName = animName;
            TrackEntry track = null;
            var anim = m_SpineAnim.skeleton.Data.FindAnimation(animName);
            if (anim == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(animName))
            {
                track = PlayAnim(animName, loop, null, speed);
                track.MixDuration = 0.1f;
            }

            return track;
        }

        private TrackEntry PlayAnim(string animName, bool loop, Action callback, float speed = 1)
        {
            m_SpineAnim.AnimationState.TimeScale = speed;
            var track = m_SpineAnim.AnimationState.SetAnimation(0, animName, loop);
            if (loop == false && callback != null)
            {
                track.Complete += (track) =>
                {
                    callback.Invoke();
                };
            }
            return track;
        }

        public void PauseAnim()
        {
            //m_Animator.speed = 0;
            if (m_SpineAnim != null)
                m_SpineAnim.AnimationState.TimeScale = 0;
        }

        public void ResumeAnim()
        {
            //m_Animator.speed = 1;
            if (m_SpineAnim != null)
                m_SpineAnim.AnimationState.TimeScale = 1;
        }

        public void PlayIdleAnim()
        {
            if (!m_Controller.IsAlive())
                return;

            SetAnimState(EnemyAnimState.Idle);
            PlayAnim(m_AnimPrefix + m_IdleAnimName, true);
        }

        public void PlayMoveAnim()
        {
            if (!m_Controller.IsAlive())
                return;

            SetAnimState(EnemyAnimState.Move);
            PlayAnim(m_AnimPrefix + m_MoveAnimName, true);
        }

        private void PlayDeadAnim()
        {
            //Log.i("Enemy play dead anim");
            SetAnimState(EnemyAnimState.Dead);
            PlayAnim(m_AnimPrefix + "Die", false);
        }

        public void PlayAttackAnim(Action onAtkTriggered, Action onEnd)
        {
            if (!m_Controller.IsAlive())
                return;

            SetAnimState(EnemyAnimState.Attack);

            var track = PlayAnim(m_AnimPrefix + "Attack", false, 1.5f);

            if (track == null)
                return;

            track.Event += (trackEntry, e) =>
                {
                    if (e.Data.Name.Equals("evt_skill_start") || e.Data.Name.Equals("evt_attack"))
                    {
                        onAtkTriggered?.Invoke();
                    }
                };
            track.Complete += (track) =>
                {
                    onEnd?.Invoke();
                    PlayIdleAnim();
                };
        }

        private void SetAnimState(EnemyAnimState state)
        {
            m_Controller.AnimState = state;
        }
    }
}