using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData
{

}

[System.Serializable]
public class Data_Player : InGameData
{
    public int currentStage;
    public Vector3 currentPosition;
    public float currentHP;

}

[System.Serializable]
public class Data_Child : InGameData
{
    public int currentStage;
    public Vector3 currentPosition; // 반딧불이 상태가 아닐 때에 사용합니다.
    public bool isDie; //빛의 폭발로 죽어있는 상태냐?
    public bool isFriend; // 반딧불이 상태냐?
    public bool isFriended; //반딧불이 상태였던 적이 있냐?

    public string name;
    public string diaryData;
}
public class Data_ChildList : InGameData
{
    public List<Data_Child> childDataList;
}