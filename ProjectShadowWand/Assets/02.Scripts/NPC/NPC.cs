using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Tooltip("NPC와 상호작용 할 수 있는 상태인가?")]
    public bool canInteract;

    [Tooltip("말하고 있는 상태인가?")]
    public bool isTalking;
    public int currentTalkCode;

    [Tooltip("바라보고 있는 방향 등")]
    public eDirection direction;

    protected Transform myTransform;
    protected Vector3 originalScale;

    public virtual void Init()
    {
        myTransform = gameObject.transform;
        originalScale = gameObject.transform.localScale;
    }
    public virtual void StartTalk()
    {

    }

    /// <summary>
    /// 플레이어의 현재 x와 내 x를 비교하여 npc의 direction을 업데이트합니다. 
    /// </summary>
    public void UpdateDirection()
    {
        if (PlayerController.Instance.playerRigidbody.position.x > myTransform.position.x ) // 오른쪽에 있음
        {
            direction = eDirection.RIGHT;
        }
        else
        {
            direction = eDirection.LEFT;

        }
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

    /// <summary>
    /// 애니메이션을 바꾸기 위한 업데이트를 합니다.
    /// </summary>
    public virtual void UpdateAnimation()
    {

    }
}
