using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public int currentTalkCode;

    public virtual void StartTalk()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ReferenceEquals(PlayerController.Instance.currentNPC, null))// NPC�� ���϶���
            {
                PlayerController.Instance.currentNPC = this; //�������� ����
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (PlayerController.Instance.currentNPC == this)//��ũ ��Ÿ�Ͱ� �����϶���
            {
                PlayerController.Instance.currentNPC = null;
            }
        }
    }
}
