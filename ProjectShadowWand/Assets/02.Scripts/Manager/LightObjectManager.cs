using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObjectManager : MonoBehaviour
{
    public static LightObjectManager Instance;

    public List<LightObject> lightObjectList;
    private int lightCount;
    private int monsterCount;

    [HideInInspector]
    public bool[] shadowJudgement;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // GameObject.DontDestroyOnLoad(this.gameObject); //상황에 따라서 돈디스트로이를 없앨수도 있음.
        }
        else
        {
           // Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        //lightObjectList.AddRange(FindObjectsOfType<LightObject>());
        //Debug.Log("LightObject를 찾았어! : " + lightObjectList.Count);
    }

    private void Update()
    {
        UpdateShadowJudgment();
    }
    /// <summary>
    /// 라이트 오브젝트 리스트에 라이트 오브젝트를 추가~
    /// </summary>
    public void AddLightObjectToList(LightObject lightObj)
    {
        lightObjectList.Add(lightObj);

        Debug.LogWarning("AddLightToList");
    }
    /// <summary>
    /// 라이트오브젝트 리스트의 그림자판정을 통해 진짜 그림자 판정을 합니다.
    /// </summary>
    public void UpdateShadowJudgment()
    {

        lightCount = lightObjectList.Count;
        monsterCount = MonsterManager.Instance.monsterList.Count;

        shadowJudgement = new bool[monsterCount];


        for (int i = 0; i < lightCount; i++) //각 라이트 오브젝트의 드로우 업데이트
        {
            lightObjectList[i].UpdateShadowJudgement();
        }

        for (int i = 0; i < shadowJudgement.Length; i++) //i = 몬스터 마리수
        {
            shadowJudgement[i] = true; //우선 트루로 한 다음
            for (int j = 0; j < lightCount; j++)
            {
                if (lightObjectList[j].shadowJudgment[i]==false) //만약 하나라도 false면 
                {
                    shadowJudgement[i] = false;
                    break;
                }
            }
        }

        MonsterManager.Instance.UpdateShadowJudgement(shadowJudgement);

        for (int i = 0; i < monsterCount; i++) // 몬스터 리스트의 몬스터들의 판정 정보를 업데이트
        {
            MonsterManager.Instance.monsterList[i].inShadow = shadowJudgement[i];
        }
        
    }
}
