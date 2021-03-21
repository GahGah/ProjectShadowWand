using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;
public class UIDiary : UIBase
{


    public Text childName;
    public Text childAge;
    public Text childRelationship;

    /// <summary>
    /// 활성화되었을 때 UIManager의 딕셔너리에 추가하던지 말던지 합니다.
    /// </summary>
    private void OnEnable()
    {
        uiType = eUItype.DIARY;
        DictionaryCheck();
        if (UIManager.Instance.uiDiary != this)
        {
            UIManager.Instance.uiDiary = this;
        }
        UpdateDiary();
    }

    public void UpdateDiary()
    {
        if (SaveLoadManager.Instance.isLoad)
        {
            Data_Child kora = new Data_Child();
            SaveLoadManager.Instance.currentData_ChildDict.TryGetValue(eChildType.TEME, out kora);
            if (kora==null)
            {
                Debug.LogError("?");
            }
            childName.text = kora.name;
            childAge.text = kora.age;
            childRelationship.text = kora.diaryData;
        }
        else
        {
            Debug.LogError("아직 로드를 실행하지 ㅇ낳았습니다.");
        }


    }
}
