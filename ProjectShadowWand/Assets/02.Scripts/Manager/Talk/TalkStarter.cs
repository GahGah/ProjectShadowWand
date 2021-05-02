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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (PlayerController.Instance.talkStater == null)// ��ũ ��Ÿ�Ͱ� ���϶���
        {
            PlayerController.Instance.talkStater = this; //�������� ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (PlayerController.Instance.talkStater == this)//��ũ ��Ÿ�Ͱ� �����϶���
        {
            PlayerController.Instance.talkStater = null;
        }
    }
}
