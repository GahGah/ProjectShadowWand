using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    Skill_WindGlide skillWindGilde;
    Skill_LightningShock skillLightningShock;

    List<Skill> skillList;

    private bool unlockWind;
    private bool unlockLightning;

    public void Init()
    {
        CheckSkills();

    }

    /// <summary>
    /// unlock ���ο� ���� ����Ʈ�� ��ų�� �߰��մϴ�.
    /// </summary>
    public void CheckSkills()
    {
        skillList = new List<Skill>();

        if (unlockLightning == true)
        {
            skillLightningShock = new Skill_LightningShock();
            skillList.Add(skillLightningShock);
        }

        if (unlockWind == true)
        {
            skillWindGilde = new Skill_WindGlide();
            skillList.Add(skillWindGilde);
        }
    }


    public void Execute()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].Execute();
        }
    }

    public void PhysicsExcute()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].PhysicsExecute();
        }

    }
}
