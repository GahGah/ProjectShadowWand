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
            if (ReferenceEquals(PlayerController.Instance.currentNPC, null))// NPC가 널일때만
            {
                PlayerController.Instance.currentNPC = this; //본인으로 설정
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
