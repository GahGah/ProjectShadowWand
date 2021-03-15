using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public List<Monster> monsterList;
    public bool[] shadowJudgement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          // GameObject.DontDestroyOnLoad(this.gameObject); // 씬 로딩을 할 때(옮겨다닐 때) 지우지마라 
        }
        else
        {
          //  Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        //monsterList.AddRange(FindObjectsOfType<Monster>());
        //Debug.Log("몬스터를 찾았어! : " + monsterList.Count);

    }

    /// <summary>
    /// 몬스터 리스트에 몬스터를 추가~
    /// </summary>
    public void AddMonsterToList(Monster monster) //몬스터 리스트에 몬스터를 추가합니다.
    {
        monsterList.Add(monster);

        Debug.LogWarning("AddMonsterToList");
    }


    public void RemoveMonsterToList(Monster monster) //몬스터 리스트에 몬스터를 추가합니다.
    {
        monsterList.Remove(monster);

        Debug.LogWarning("RemoveMonsterToList...");
    }


    /// <summary>
    /// 몬스터들의 그림자 판정 정보를 업데이트합니다.
    /// </summary>
    public void UpdateShadowJudgement(bool[] arr)
    {
        shadowJudgement = arr;

    }

}
