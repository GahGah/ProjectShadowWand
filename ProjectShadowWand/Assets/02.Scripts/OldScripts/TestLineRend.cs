using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineRend : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 moveInput;
    public float angle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        angle = angle + 90f;
    }
    private void Update()
    {
        if (InputManager.Instance.buttonMoveRight.isPressed)
        {
            moveInput = Vector2.right;
        }
        else if (InputManager.Instance.buttonMoveLeft.isPressed)
        {
            moveInput = Vector2.left;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {

        //var rot = Quaternion.AngleAxis(angle, Vector3.right);
        Vector2 dir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle * moveInput.x), Mathf.Cos(Mathf.Deg2Rad * angle * moveInput.x));
        if (moveInput != Vector2.zero)
        {
            rb.velocity = dir.normalized * 3f;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }


    }
}
