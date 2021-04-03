using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tornado : WeatherInteractionObject
{
    public PlayerController player;

    public bool isPlayerIn;
    private void Awake()
    {
    }
    public override void Init()
    {
        base.Init();
    }
    private void Update()
    {
        ChangeState();
        Exectue();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AreaEffector"))
        {
            Debug.Log("아리아 이펙터");
        }
    }
}
