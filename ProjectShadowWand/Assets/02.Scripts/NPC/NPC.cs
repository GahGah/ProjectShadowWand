using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Tooltip("NPC와 상호작용 할 수 있는 상태인가?")]
    public bool canInteract;

    public int currentTalkCode;

    public virtual void StartTalk()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canInteract)
            {
                if (ReferenceEquals(PlayerController.Instance.currentNPC, null))// NPC가 널일때만
                {
                    PlayerController.Instance.currentNPC = this; //본인으로 설정
                }
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (PlayerController.Instance.currentNPC == this)//토크 스타터가 본인일때만
            {
                PlayerController.Instance.currentNPC = null;
            }
        }
    }
}
