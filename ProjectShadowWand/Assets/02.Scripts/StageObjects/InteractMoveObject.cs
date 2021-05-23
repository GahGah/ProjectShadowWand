using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 움직이면 
/// </summary>
public class InteractMoveObject : MonoBehaviour
{
    public bool isMove;
    private Rigidbody2D rb;
    public Collider2D col;
    private void Start()
    {
        Physics2D.IgnoreCollision(col, PlayerController.Instance.playerCollider, true);
        rb = GetComponent<Rigidbody2D>();
        StageManager.Instance.AddTrashList(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isMove)
            {
                rb.velocity = PlayerController.Instance.movementInput * 3f;
            }

        }
    }
}
