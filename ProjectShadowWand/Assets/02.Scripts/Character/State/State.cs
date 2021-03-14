using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State의 기본이 되는 녀석~
public class State
{
    public bool isDebugMod;
    public virtual void Enter()
    {
        Debug.Log("State Enter");
    }
    public virtual void Execute()
    { 
    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public virtual void PhysicsExecute() { } 
    public virtual void Exit()
    { 
        Debug.Log("State Exit...");
    }

    public virtual void HandleInput() { }

    public void Log(object obj)
    {
        if (!isDebugMod)
        {
            Debug.Log(obj);
        }

    }
}
