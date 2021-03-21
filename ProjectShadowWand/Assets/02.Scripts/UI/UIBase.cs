using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{

    [Tooltip("현재 직접 구분해서 type을 정하는 기능이 없습니다. 직접 넣어라!!")]
    public eUItype uiType;
    private void Start()
    {
        Debug.Log("Start!");
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    /// <summary>
    /// UI가 켜졌을 때를 말합니다. 근데 이게 쓰이려나
    /// </summary>
    public virtual void OnActive()
    {

    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    /// <summary>
    /// 유아이매니저의 딕셔너리 안에 들어있는지 확인합니다. 없으면 추가합니다.
    /// </summary>
    protected void DictionaryCheck()
    {
        if (UIManager.Instance.uiDicitonary.ContainsKey(uiType) == false)//목록에 안들어가 있다면
        {
            Debug.Log("Add..." + this);
            UIManager.Instance.AddToDictionary(this);
        }
        else
        {
            Debug.Log(gameObject.name + "는 이미 추가되어있습니다.");
        }
    }
}
