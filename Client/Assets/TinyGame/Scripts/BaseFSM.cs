/* ***********************************************
* Discribe：
* Author：daidai 
* CreateTime：2022-05-10 10:34:01
* ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IFSM
{
    FSMState CurState { get; set; }

    void StartFSM(FSMState initState);
}

public abstract class FSMState
{
    protected IFSM m_Owner;
    public FSMState(IFSM fsm)
    {
        m_Owner = fsm;
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public virtual void OnUpdate() { }

    public void ChangeState(FSMState newState)
    {
        if (newState == null)
        {
            throw new System.Exception("newState is null!");
        }

        var curState = m_Owner.CurState;
        if (curState == null)
        {
            throw new System.Exception("curState is null! 请先调用StartFSM进行启动!");
        }

        if (m_Owner != newState.m_Owner)
        {
            throw new System.Exception("curState和newState不是同一个 IFSM !");
        }

        if (curState == newState)
        {
            Log.Info("前后状态一致!");
            return;
        }

        m_Owner.CurState.OnExit();
        m_Owner.CurState = newState;
        m_Owner.CurState.OnEnter();
    }
}

public class EmptyState : FSMState
{
    public EmptyState(IFSM fsm) : base(fsm) { }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}

