using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal; // Light2D를 가져오기 위해...

public class LightObjectExplosion : LightObject
{

    List<Monster> killMonsterList;

    private float timer;
    private bool isUsed;//사용했냐?

    private bool isEndExplosionEnter;
    private bool isEndExplosionExit;

    private bool isEndKill;

    private bool isEnd;

    private float limitIntensity;
    protected override void StartSetting()
    {
        base.StartSetting();
        light2D = GetComponent<Light2D>();
        timer = 0f;
        isUsed = false;
        isEndExplosionEnter= false;
        isEndExplosionExit= false;
        isEndKill = false;
        isEnd = false;
    }
    public override void LateUpdateLightObject()
    {
        if (isUsed == false) // 아직 미사용 상태라면
        {
            Debug.Log("진입");
            isUsed = true;
            StartCoroutine(ProcessExplosionEnter());
        }
        else if (isEndExplosionEnter == true && isEndKill == false)
        {
            Debug.Log("몬스터 제거");
            CheckKillMonster();
            KillMonsters();
            isEndKill = true;
        }
        else if (isEndKill == true && isEnd == false)
        {
            Debug.Log("폭발 끝");
            isEnd = true;
            StartCoroutine(ProcessExplosionExit());
        }
        else if (isEndExplosionExit==true)
        {
            KillLightObject();
        }
    }

    IEnumerator ProcessExplosionEnter()
    {

        light2D.intensity = 0f;
        timer = 0f;
        limitIntensity = 5f;
        float changeSpeed = 3f;
        while (light2D.intensity < limitIntensity)
        {
            timer += Time.unscaledDeltaTime * changeSpeed;
            light2D.intensity = timer;
            yield return null;
        }

        isEndExplosionEnter = true;
    }

    IEnumerator ProcessExplosionExit()
    {
        float changeSpeed = 3f;
        while (light2D.intensity > 0f)
        {
            timer -= Time.unscaledDeltaTime * changeSpeed;
            Mathf.Clamp(timer, 0f, limitIntensity);
            light2D.intensity = timer;
            yield return null;
        }
        isEndExplosionExit = true;
    }

    public void CheckKillMonster()
    {
        killMonsterList = new List<Monster>();

        int _monsterCount = MonsterManager.Instance.monsterList.Count;
        for (int i = 0; i < _monsterCount; i++)
        {
            bool isInScreen = CameraManager.Instance.CheckThisObjectInScreen(MonsterManager.Instance.monsterList[i].gameObject);
            if (isInScreen == true)// 카메라 안에 들어가있으면
            {
                if (MonsterManager.Instance.monsterList[i] == null)
                {
                    Debug.LogError("???");
                }
                killMonsterList.Add(MonsterManager.Instance.monsterList[i]);
            }
        }
        Debug.LogWarning("KillMonsterCount : " + killMonsterList.Count);
    }

    public void KillMonsters()
    {
        int _killMonsterCount = killMonsterList.Count;
        for (int i = 0; i < _killMonsterCount; i++)
        {
            killMonsterList[i].KillMonster();
        }

        Debug.LogWarning("빛의 폭발! 남은 몬스터 수 : " + MonsterManager.Instance.monsterList.Count);
    }
}
