using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//밟으면 점프할 수 있는 오브젝트입니다.
public class Jumpplesia : Plant
{
    public float jumpPower;
    public int playerMask;
    private void Start()
    {
        if (jumpPower == 0f)
        {
            jumpPower = 5f;
        }
        playerMask = LayerMask.NameToLayer("Player");
        //| (1 << LayerMask.NameToLayer("Default"));
        //  noPlayerMask = ~noPlayerMask;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var dir = PlayerController.Instance.playerRigidbody.position - (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 10f, playerMask);

            Debug.DrawRay(transform.position, dir * 10f, Color.black, 0.5f);
            var angle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.Log("노말 : " + hit.normal + ", 앵글 :  " + angle);
            if (angle == 0f)
            {
                PlayerController.Instance.SetExtraForce(new Vector2(0f, jumpPower));
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController.Instance.SetExtraForce(Vector2.zero);
        }

    }
}
