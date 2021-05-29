using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Tooltip("NPC�� ��ȣ�ۿ� �� �� �ִ� �����ΰ�?")]
    public bool canInteract;

    [Tooltip("���ϰ� �ִ� �����ΰ�?")]
    public bool isTalking;
    public int currentTalkCode;

    [Tooltip("�ٶ󺸰� �ִ� ���� ��")]
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
    /// �÷��̾��� ���� x�� �� x�� ���Ͽ� npc�� direction�� ������Ʈ�մϴ�. 
    /// </summary>
    public void UpdateDirection()
    {
        if (PlayerController.Instance.playerRigidbody.position.x > myTransform.position.x ) // �����ʿ� ����
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
                if (ReferenceEquals(PlayerController.Instance.currentNPC, null))// NPC�� ���϶���
                {
                    PlayerController.Instance.currentNPC = this; //�������� ����
                }
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

    /// <summary>
    /// �ִϸ��̼��� �ٲٱ� ���� ������Ʈ�� �մϴ�.
    /// </summary>
    public virtual void UpdateAnimation()
    {

    }
}
