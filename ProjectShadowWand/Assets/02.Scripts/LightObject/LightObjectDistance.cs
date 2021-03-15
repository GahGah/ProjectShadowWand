using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // Light2D를 가져오기 위해...
public class LightObjectDistance : LightObject
{
    [Tooltip("distanceMode가 true인 경우 설정한 거리만큼만 판정합니다.")]
    public float limitDistance;
    public override void UpdateShadowJudgement()
    {
        if (shadowJudgment.Length != MonsterManager.Instance.monsterList.Count) //갯수가 다르면 
        {
            shadowJudgment = new bool[MonsterManager.Instance.monsterList.Count]; //새로 해주기.
        }

        monsterCount = MonsterManager.Instance.monsterList.Count;

        for (int i = 0; i < monsterCount; i++)
        {
            Monster nowMonster = MonsterManager.Instance.monsterList[i];


            Vector2 dir = nowMonster.rb.position - new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(transform.position, dir.normalized * limitDistance, Color.cyan, 0.5f);
            nowMonster.hit = Physics2D.Raycast(transform.position, dir.normalized, limitDistance);

            bool tempHits = false;
            if (nowMonster.hit.collider != null)
            {
                if (nowMonster.hit.collider.CompareTag("Monster"))
                {
                    tempHits = true;
                }

            }
            nowMonster.UpdateHitsLog();

            shadowJudgment[i] = !tempHits;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, limitDistance);

    }
    protected override void StartSetting()
    {
        base.StartSetting();
        light2D = GetComponent<Light2D>();
        if (light2D != null && light2D.lightType == Light2D.LightType.Parametric)
        {
            limitDistance = light2D.shapeLightParametricRadius;
        }
    }
}
