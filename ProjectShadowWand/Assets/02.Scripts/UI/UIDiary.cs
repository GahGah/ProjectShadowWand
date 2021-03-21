using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDiary : UIBase
{
    private void OnEnable()
    {
        uiType = eUItype.DIARY;
        DictionaryCheck();
        if (UIManager.Instance.uiDiary != this)
        {
            UIManager.Instance.uiDiary = this;
        }

    }
}
