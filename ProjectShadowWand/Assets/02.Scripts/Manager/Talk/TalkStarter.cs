using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC와의 대화를 시작하기 위해 필요함!!
/// </summary>
public class TalkStarter : MonoBehaviour
{
    [Tooltip("해당하는 대화의 파일 이름을 씁니다.")]
    public string talkFileName;

    [Tooltip("대화를 시작할 토크 코드입니다.")]
    public int talkCode;

    [Tooltip("대화를 끝낸 상태인가?")]
    public bool isEnd;
    public bool isStart;
    /// <summary>
    /// talkCode에 있는 대화를 시작합니다.
    /// </summary>
    public void StartTalk()
    {
        if (TalkSystemManager.Instance.isTalkStart == false)
        {
            ///TalkSystemManager.Instance.currentTalkStarter = this;
            //TalkSystemManager.Instance.StartGoTalk(talkCode,);

        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    //if (PlayerController.Instance.talkStater == null)// 토크 스타터가 널일때만
    //    //{
    //    //    PlayerController.Instance.talkStater = this; //본인으로 설정
    //    //}
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //    //if (PlayerController.Instance.talkStater == this)//토크 스타터가 본인일때만
    //    //{
    //    //    PlayerController.Instance.talkStater = null;
    //    //}
    //}
}
