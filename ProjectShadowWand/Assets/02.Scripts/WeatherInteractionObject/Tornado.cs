using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tornado : WeatherInteractionObject
{
    public PlayerController player;

    [Header("ÁÂ,¿ì·Î ¿òÁ÷ÀÌ´Â Èû")]
    public float movePower;

    private bool isPlayerIn;

    private Rigidbody2D rb;

    public LayerMask areaMask;
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        ChangeState();
        Exectue();

        if (rb.IsTouchingLayers(LayerMask.GetMask("AreaEffector")))
        {
            //rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
           // rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(movePower, 0f);
        }
    
    }
    public override void Exectue()
    {
        base.Exectue();
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }

    public override void ProcessRainy()
    {

    }
    public override void ProcessSunny()
    {
    }


}
