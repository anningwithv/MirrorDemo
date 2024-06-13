using UnityEngine;
using System.Collections;
using ProjectX;

public class FSMState<T>
{
    public virtual string stateName
    {
        get { return this.GetType().Name; }
    }

    public virtual void Enter(T entity)
    {
        //Log.i("Enter state: " + stateName);
    }

    public virtual void Execute(T entity, float dt)
    {

    }

    public virtual void Exit(T entity)
    {
        //Log.i("Exit state: " + stateName);
    }

    public virtual void OnMsg(T entity, int key, params object[] args)
    {

    }
}


