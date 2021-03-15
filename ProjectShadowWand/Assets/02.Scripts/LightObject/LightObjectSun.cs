using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObjectSun : LightObject
{

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

            nowMonster.directions = new Vector2[] //거꾸로 하면 빛에서부터 몬스터로의 방향을 구하지 않을까? 싶어서...
            {
                nowMonster.path[0] - transform.position,
                nowMonster.path[1] - transform.position,
                nowMonster.path[2] - transform.position,
                nowMonster.path[3] - transform.position,
            };

            for (int j = 0; j < 4; j++) //라이트에서 몬스터를 향해 레이캐스트
            {
                Debug.DrawRay(transform.position, nowMonster.directions[j], nowMonster.colors[j], 0.5f);
                nowMonster.hits[j] = Physics2D.Raycast(transform.position, nowMonster.directions[j], nowMonster.directions[j].magnitude, layerMask);
            }

            //순서 :
            // 3 2
            // 0 1


            nowMonster.UpdateHitsLog();

            shadowJudgment[i] = nowMonster.isAllHitsTrue();
        }
    }
}
