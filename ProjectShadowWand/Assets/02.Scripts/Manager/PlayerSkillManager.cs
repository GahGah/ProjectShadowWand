using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public Skill_WindGlide skillWindGilde;
    public Skill_LightningShock skillLightningShock;

    List<Skill> skillList = null;

    public bool unlockWind;
    public bool unlockLightning;

    public void Init()
    {
        CheckSkills();
    }

    /// <summary>
    /// unlock ���ο� ���� ����Ʈ�� ��ų�� �߰��մϴ�.
    /// </summary>
    public void CheckSkills()
    {
        if (skillList != null)
        {
            skillList.Clear();

        }
        skillList = new List<Skill>();

        if (unlockLightning == true)
        {
            skillLightningShock = new Skill_LightningShock(PlayerController.Instance);
            skillList.Add(skillLightningShock);
        }

        if (unlockWind == true)
        {
            skillWindGilde = new Skill_WindGlide(PlayerController.Instance);
            skillList.Add(skillWindGilde);
        }
    }


    public void Execute()
    {
        if (skillList != null)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].Execute();
            }
        }

    }

    public void PhysicsExcute()
    {
        if (skillList != null)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].PhysicsExecute();
            }
        }
    }

    /// <summary>
    /// ��ų�� �ر��ϰ� ȹ���մϴ�.�̹� ������ �ƹ��͵� ��������
    /// </summary>
    public void UnlockSkill(eSkill _skill)
    {
        switch (_skill)
        {
            case eSkill.WINDGILDE:
                if (unlockWind == false) //�ر����� ���� ���¶��
                {
                    unlockWind = true;
                    skillWindGilde = new Skill_WindGlide(PlayerController.Instance);
                    skillList.Add(skillWindGilde);
                }
                break;

            case eSkill.LIGHTNINGSHOCK:
                if (unlockLightning == false) //�ر����� ���� ���¶��
                {
                    unlockWind = true;
                    skillLightningShock = new Skill_LightningShock(PlayerController.Instance);
                    skillList.Add(skillLightningShock);
                }

                break;

            default:
                break;
        }
    }
}
