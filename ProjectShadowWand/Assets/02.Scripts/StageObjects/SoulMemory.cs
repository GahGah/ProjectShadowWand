using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;


    [Tooltip("상호작용 한 적 있는지를 뜻합니다.")]
    public bool isTake;
    [Tooltip("isEnd가 true일 경우 UI에 사념이 추가됩니다.")]
    public bool isEnd;

    public int currentTalkCode;
    private void Awake()
    {
        isTake = false;
        isEnd = false;
    }



    private void Start()
    {
        stageManager = StageManager.Instance;

        if (stageManager.soulMemoryList.Contains(this) == false) // 리스트 안에 자기가 안들어가있다면
        {
            stageManager.AddSoulMemory(this);
            Debug.LogError(gameObject.name + " : 스테이지 매니저 사념 리스트에 제가 없길래 넣었습니다. 문제는 없어졌겠지만 오류날 수도 있으니까 직접 넣으세요.");

        }
    }

    public void TakeSoulMemory()
    {
        isTake = true;
        TalkSystemManager.Instance.StartReadSoulMemory(currentTalkCode, this);
        //gameObject.SetActive(false);
        //isEnd = true;
        //stageManager.CheckClearCondition_SoulMemory();
    }

    /// <summary>
    /// 소울 메모리의 모습을 없앱니다. 일단은 SetActivefalse.
    /// </summary>
    public void DisappearSoulMemory()
    {
        gameObject.SetActive(false);
        //stageManager.CheckClearCondition_SoulMemory();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ReferenceEquals(PlayerController.Instance.currentSoulMemory, null))// 소울메모리가 널일때만
            {
                PlayerController.Instance.currentSoulMemory = this; //본인으로 설정
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.currentSoulMemory == this)//소울 메모리가 본인일때만
            {
                PlayerController.Instance.currentSoulMemory = null; // null로 해버리기
            }
        }
    }
}
