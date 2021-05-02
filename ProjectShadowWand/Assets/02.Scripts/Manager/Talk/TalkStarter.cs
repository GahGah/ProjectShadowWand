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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (PlayerController.Instance.talkStater == null)// 토크 스타터가 널일때만
        {
            PlayerController.Instance.talkStater = this; //본인으로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (PlayerController.Instance.talkStater == this)//토크 스타터가 본인일때만
        {
            PlayerController.Instance.talkStater = null;
        }
    }
}
