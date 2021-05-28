using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{

    public bool isOn;
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        player = PlayerController.Instance;
    }


    public override void DoInteract()
    {
        isOn = !isOn;
    }

    public override void SetTouchedObject(bool _b)
    {
        base.SetTouchedObject(_b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(false);
        }

    }

}
