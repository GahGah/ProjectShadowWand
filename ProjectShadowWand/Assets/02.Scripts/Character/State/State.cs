using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State의 기본이 되는 녀석~
public class State : MonoBehaviour
{
    public virtual void Enter() { }
    public virtual void Execute() { }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public virtual void PhysicsExecute() { } 
    public virtual void Exit() { }

    public virtual void HandleInput() { }
}
