using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected PlayerController player;
    public virtual void Init()
    {

    }

    /// <summary>
    /// 스킬이 시작될 때 호출됩니다.
    /// </summary>
    public virtual void StartSkill()
    {
    }
    /// <summary>
    /// 스킬이 종료될 때 호출됩니다.
    /// </summary>
    public virtual void EndSkill()
    {

    }
    public virtual void Execute()
    {

    }

    public virtual void PhysicsExecute()
    {

    }
}
