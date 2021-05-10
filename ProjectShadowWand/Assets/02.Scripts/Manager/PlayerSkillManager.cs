using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public Skill_WindGlide skillWindGilde;
    public Skill_LightningShock skillLightningShock;
    public Skill_WaterWave skillWaterWave;

    [Header("�ٶ�")]
    public GameObject windEffect;


    [Header("��")]

    [Tooltip("CastStart��ġ")]
    public Transform waterPosition;

    [Tooltip("�� ��ų ���� ����Ʈ")]
    public GameObject waterEffect_Set;

    [Tooltip("�� ��ų �ߵ� ����Ʈ")]
    public GameObject waterEffect_Splash;


    List<Skill> skillList = null;

    public bool unlockWind;
    public bool unlockLightning;
    public bool unlockWater;
    public void Init()
    {
        CheckSkills();
        if (skillList != null)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].Init();
            }
        }
        windEffect.SetActive(false);
    }


    public void WindInit()
    {
        skillWindGilde.windEffect = windEffect;
        skillWindGilde.windAnimator =  windEffect.GetComponent<Animator>();
        skillWindGilde.windAnimatorTornadoBlend = Animator.StringToHash("TornadoBlend");
    }


    public void WaterInit()
    {
        skillWaterWave.waterEffect_Set = waterEffect_Set;
        skillWaterWave.waterEffect_Splash = waterEffect_Splash;
        skillWaterWave.waterPosition = waterPosition;
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

        if (unlockWind == true)
        {
            skillWindGilde = new Skill_WindGlide(PlayerController.Instance);

            WindInit();

            skillList.Add(skillWindGilde);
        }
        if (unlockWater ==true)
        {
            skillWaterWave = new Skill_WaterWave(PlayerController.Instance);

            WaterInit();

            skillList.Add(skillWaterWave);
        }
        if (unlockLightning == true)
        {
            skillLightningShock = new Skill_LightningShock(PlayerController.Instance);
            skillList.Add(skillLightningShock);
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
    public void UnlockWind()
    {

        UnlockSkill(eSkill.WINDGILDE);
    }

    public void UnlockLightning()
    {
        UnlockSkill(eSkill.LIGHTNINGSHOCK);
    }

    public void UnlockWater()
    {
        UnlockSkill(eSkill.WATERWAVE);
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

                    WindInit();
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

            case eSkill.WATERWAVE:
                if (unlockWater == false) //�ر����� ���� ���¶��
                {
                    unlockWater = true;
                    skillWaterWave = new Skill_WaterWave(PlayerController.Instance);
                    WaterInit();
                    skillList.Add(skillWaterWave);
                }
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(waterPosition.position, PlayerController.Instance.waterSize);
        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(waterPosition.position, PlayerController.Instance.waterDirection * PlayerController.Instance.waterDistance);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(waterPosition.position + (Vector3)PlayerController.Instance.waterDirection * PlayerController.Instance.waterDistance, PlayerController.Instance.waterSize);


    }

}
