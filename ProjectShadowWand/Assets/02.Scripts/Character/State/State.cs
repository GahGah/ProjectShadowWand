using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eSTATE // 상태를 정의해놓습니다.
{
    PLAYER_DEFAULT,
    PLAYER_JUMP,
    PLAYER_AIR,
    PLAYER_DIE,

    /// <summary>
    /// 몬스터가 그림자 안에 있고, 추격 상태가 아닐 때를 의미합니다.
    /// </summary>
    MONSTER_DEFAULT,
    /// <summary>
    /// 몬스터가 그림자 안에 있고, 추격 상태일때를 의미합니다.
    /// </summary>
    MONSTER_CHASE,

    /// <summary>
    /// 몬스터가 그림자 밖에 나왔을 때를 의미합니다.
    /// </summary>
    MONSTER_OUTSHADOW,

    /// <summary>
    /// 몬스터가 뒈짖
    /// </summary>
    MONSTER_DIE

}

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
        if (isDebugMod)
        {
            Debug.Log(obj);
        }

    }
}
