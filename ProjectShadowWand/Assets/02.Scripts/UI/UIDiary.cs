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

    public List<DiaryPage> diaryPageList = new List<DiaryPage>();

    /// <summary>
    /// 현재 다이어리의 페이지입니다.
    /// </summary>
    public DiaryPage currentsDiaryPage;
    /// <summary>
    /// 활성화되었을 때 UIManager의 딕셔너리에 추가하던지 말던지 합니다.
    /// </summary>
    private void Awake()
    {
        Init();
    }

    public override void Init()
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
            if (kora == null)
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

    /// <summary>
    /// 지정된 페이지로 이동합니다.
    /// </summary>
    /// <param name="_index">페이지의 인덱스입니다.</param>
    public void MovePage(int _index)
    {
        //현재 소지하고 있는 다이어리 페이지_[index].childType
        //딕셔러리에서
    }
}


/// <summary>
/// 다이어리의 한 페이지를 의미하는 클래스입니다.
/// </summary>
public class DiaryPage
{
    /// <summary>
    /// 이 페이지는 어떤 child의 페이지인가?
    /// </summary>
    public eChildType childType;

    public Text childName;
    public Image childImage;
    ///ETC...
}