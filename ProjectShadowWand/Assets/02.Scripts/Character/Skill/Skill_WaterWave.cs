using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WaterWave : Skill
{


    RaycastHit2D[] hits;

    public GameObject startPos;
    //[Header("판정 크기")]
    //public Vector2 size;

    //[Header("판정 범위")]
    //public float maxDistance = 4f;

    //[Header("지속 시간")]
    //public float activeTime;

    public bool hit = false;

    public int plantLayerMask;


    public Skill_WaterWave(PlayerController _p)
    {
        player = _p;
    }


    public override void Init()
    {
        if (player.waterActiveTime <= 0f)
        {
            player.waterActiveTime = 2f;
        }
        startPos = player.gameObject;
        // plantLayerMask = LayerMask.NameToLayer("Plant");
        plantLayerMask = (1 << LayerMask.NameToLayer("Plant"));
    }
    public override void Execute()
    {
        UpdateWaterDirection();
        if (InputManager.Instance.buttonSkillWater.wasPressedThisFrame)
        {
            if (player.WaterCoroutine == null)
            {
                player.WaterCoroutine = ProcessWater();
                player.StartCoroutine(player.WaterCoroutine);
            }
        }
    }

    public void UpdateWaterDirection()
    {
        if (player.isRight)
        {
            player.waterDirection = Vector2.right;
        }
        else
        {
            player.waterDirection = Vector2.left;
        }
    }
    public override void PhysicsExecute()
    {
    }

    IEnumerator ProcessWater()
    {
        Debug.Log("StartWater");

        var timer = 0f;
        while (timer < player.waterActiveTime)
        {
            timer += Time.deltaTime;

            hits = Physics2D.BoxCastAll(startPos.transform.position, player.waterSize, 0f, player.waterDirection, player.waterDistance, plantLayerMask);


            hit = false;

            foreach (var item in hits)
            {
                var test = item.collider.GetComponent<GrowableObject>();
                if (test != null)
                {
                    Debug.Log(test.name);
                    test.OnWater();
                }
                if (item && hit == false)
                {
                    hit = true;
                }

            }
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("End Water");
        player.WaterCoroutine = null;
    }

}
