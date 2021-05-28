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
    /// ��ų�� ���۵� �� ȣ��˴ϴ�.
    /// </summary>
    public virtual void StartSkill()
    {
    }
    /// <summary>
    /// ��ų�� ����� �� ȣ��˴ϴ�.
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
