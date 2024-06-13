using GameFrame;
using Mirror;
using ProjectX.Logic;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : NetworkBehaviour, ICharacterComOwner
{
    public Transform Transform => transform;
    public MonoBehaviour Mono { get; set; }
    public HealthCom HealthCom { get; private set; }
    public Vector3 MoveDir { get; set; }
    public Rigidbody2D Rgb { get; private set; }

    protected Dictionary<int, CharacterComponent> m_ComDic;
    protected bool m_IsFacingRight;
    protected SkeletonAnimation m_SpineAnim;

    protected virtual void Awake()
    {
        try
        {
            m_ComDic = new();

            HealthCom = RegisterCom<HealthCom>();

            Mono = GetComponent<MonoBehaviour>();
            Rgb = GetComponent<Rigidbody2D>();
            m_SpineAnim = GetComponentInChildren<SkeletonAnimation>();
        }
        catch (Exception e)
        {
            Log.e("Catch exception: " + e.ToString() + " ----StackTrace:" + e.StackTrace);
        }
    }

    protected virtual void Start()
    {
        try
        {
            foreach (var item in m_ComDic)
            {
                item.Value.OnStart();
            }
        }
        catch (Exception e)
        {
            Log.e("Catch exception: " + e.ToString() + " ----StackTrace:" + e.StackTrace);
        }
    }

    protected virtual void Update()
    {
        if (m_ComDic == null || m_ComDic.Count == 0)
            return;

        try
        {
            foreach (var item in m_ComDic)
            {
                item.Value.OnUpdate(Time.deltaTime);
            }
        }
        catch (Exception e)
        {
            Log.e("Catch exception: " + e.ToString() + " ----StackTrace:" + e.StackTrace);
        }
    }

    protected virtual void OnEnable()
    { }

    protected virtual void OnDisable()
    { }


    public T RegisterCom<T>() where T : CharacterComponent, new ()
    {
        int key = typeof(T).GetHashCode();
        if (!m_ComDic.ContainsKey(key))
        {
            T com = new T();
            com.SetOwner(this); 
            m_ComDic.Add(key, com);
        }

        return (T)m_ComDic[key];
    }

    public void UnregisterCom<T>() where T : CharacterComponent
    {
        int key = typeof(T).GetHashCode();
        if (m_ComDic.ContainsKey(key))
        {
            m_ComDic.Remove(key);
        }
    }

    public void UnregisterAllComs()
    {
        m_ComDic?.Clear();
    }

    public T GetCom<T>() where T : CharacterComponent
    {
        int key = typeof(T).GetHashCode();
        if (m_ComDic.ContainsKey(key))
        {
            return (T)m_ComDic[key];
        }

        return null;
    }

    public virtual void OnRecycled()
    {
        if (m_ComDic != null)
        {
            for (int i = 0; i < m_ComDic.Count; i++)
            {
                m_ComDic[i].OnRecycled();
            }
        }
    }

    public virtual bool IsAlive()
    {
        return HealthCom.IsAlive();
    }

    public void MoveToPos(Vector3 targetPos)
    {
        var dir = targetPos - transform.position;
        dir = dir.normalized;
        dir.z = 0;
        MoveDir = dir;

        Rgb.velocity = dir * GetMoveSpeed();

        FaceToDir(dir);
    }

    public void FaceToDir(Vector3 dir)
    {
        if (dir.x > 0 && !m_IsFacingRight)
        {
            //Log.i("Face to right");
            m_IsFacingRight = true;
            m_SpineAnim.skeleton.ScaleX = 1;
        }
        else if (dir.x < 0 && m_IsFacingRight)
        {
            //Log.i("Face to left");
            m_IsFacingRight = false;
            m_SpineAnim.skeleton.ScaleX = -1;
        }
    }

    protected virtual float GetMoveSpeed()
    {
        return 2;
    }
}
