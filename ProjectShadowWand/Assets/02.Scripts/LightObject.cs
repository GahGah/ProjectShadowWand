using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빛을 내뿜는다고 가정하는 오브젝트입니다.
/// </summary>
public class LightObject : MonoBehaviour
{

    [Tooltip("거리에 상관없이 몬스터에게 영향을 줍니다.")]
    public bool globalMode;

    [Tooltip("globalMode가 false인 경우 설정한 거리만큼만 판정합니다.")]
    public float limitDistance;

    public int monsterCount;

    [Tooltip("해당 몬스터가 그림자에 들어가있는 상태라면 true를, 아니라면 false를 갖습니다. 인덱스는 몬스터 리스트와 동일합니다. ")]
    public bool[] shadowJudgment;

    private Mesh mesh;



    void Start()
    {
        StartSetting();
    }

    void StartSetting()
    {

        shadowJudgment = new bool[MonsterManager.Instance.monsterList.Count];
        if (LightObjectManager.Instance.lightObjectList.Contains(this) == false) //자기 자신이 안들어가있다면
        {
            LightObjectManager.Instance.AddLightObjectToList(this); //넣는다.
        }
    }
    void Update()
    {
        // AllMonsterDrawTest();
    }


    public void AllMonsterDrawTest() //몬스터 매니저의 몬스터리스트를 바탕으로 몬스터를 향해 레이를 쏜다!
    {

    }

    Mesh SpriteToMesh(Sprite sprite)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
        mesh.uv = sprite.uv;
        mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

        return mesh;
    }

    public void UpdateShadowJudgement()
    {
        if (globalMode)
        {
            if (shadowJudgment.Length != MonsterManager.Instance.monsterList.Count) //갯수가 다르면 
            {
                shadowJudgment = new bool[MonsterManager.Instance.monsterList.Count]; //새로 해주기.
            }

            monsterCount = MonsterManager.Instance.monsterList.Count;

            for (int i = 0; i < monsterCount; i++)
            {
                Monster nowMonster = MonsterManager.Instance.monsterList[i];


                //nowMonster.directions = new Vector2[] //몬스터에서부터 빛으로의 방향을 구함
                //{
                //    transform.position - nowMonster.path[0],
                //    transform.position - nowMonster.path[1],
                //    transform.position - nowMonster.path[2],
                //    transform.position - nowMonster.path[3]
                //};
                //for (int j = 0; j < 4; j++) //몬스터에서 라이트를 향해 레이캐스트
                //{
                //    Debug.DrawRay(nowMonster.path[j], nowMonster.directions[j], nowMonster.colors[j], 0.5f);
                //    nowMonster.hits[j] = Physics2D.Raycast(nowMonster.path[j], nowMonster.directions[j], nowMonster.directions[j].magnitude, nowMonster.layerMask);
                //}
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
                    nowMonster.hits[j] = Physics2D.Raycast(transform.position, nowMonster.directions[j], nowMonster.directions[j].magnitude, nowMonster.layerMask);
                }

                //순서 :
                // 3 2
                // 0 1


                nowMonster.UpdateHitsLog();

                shadowJudgment[i] = nowMonster.isAllHitsTrue();
            }
        }
        else
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
                    nowMonster.hits[j] = Physics2D.Raycast(transform.position, nowMonster.directions[j], nowMonster.directions[j].magnitude, nowMonster.layerMask);
                }

                //순서 :
                // 3 2
                // 0 1


                nowMonster.UpdateHitsLog();

                shadowJudgment[i] = nowMonster.isAllHitsTrue();
            }
        }
    }
}
