using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC���� ��ȭ�� �����ϱ� ���� �ʿ���!!
/// </summary>
public class TalkStarter : MonoBehaviour
{
    [Tooltip("�ش��ϴ� ��ȭ�� ���� �̸��� ���ϴ�.")]
    public string talkFileName;

    [Tooltip("��ȭ�� ������ ��ũ �ڵ��Դϴ�.")]
    public int talkCode;

    [Tooltip("��ȭ�� ���� �����ΰ�?")]
    public bool isEnd;
    public bool isStart;
    /// <summary>
    /// talkCode�� �ִ� ��ȭ�� �����մϴ�.
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

    //    //if (PlayerController.Instance.talkStater == null)// ��ũ ��Ÿ�Ͱ� ���϶���
    //    //{
    //    //    PlayerController.Instance.talkStater = this; //�������� ����
    //    //}
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //    //if (PlayerController.Instance.talkStater == this)//��ũ ��Ÿ�Ͱ� �����϶���
    //    //{
    //    //    PlayerController.Instance.talkStater = null;
    //    //}
    //}
}
