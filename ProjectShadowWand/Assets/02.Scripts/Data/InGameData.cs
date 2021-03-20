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
    public Data_Child() { }

    /// <summary>
    /// _data를 깊은 복사 합니다.
    /// </summary>
    /// <param name="_data">복사할 원본 데이터</param>
    public Data_Child(Data_Child _data)
    {

        currentStage = _data.currentStage;
        currentPosition = _data.currentPosition;
        isDie = _data.isDie;
        isFriend = _data.isFriend;
        isBye = _data.isBye;
        name = _data.name;
        diaryData = _data.diaryData;


    }

    public int currentStage;
    public Vector3 currentPosition; // 반딧불이 상태가 아닐 때에 사용합니다.
    public bool isDie; //빛의 폭발로 죽어있는 상태냐?
    public bool isFriend; // 반딧불이 상태냐?
    public bool isBye; //플레이어가 떠나보낸 상태냐?

    public string name;
    public string diaryData;

}
public class Data_ChildList : InGameData
{
    public List<Data_Child> childDataList;
}
