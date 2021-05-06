using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_LightningShock : Skill
{
    public Skill_LightningShock(PlayerController _p)
    {
        player = _p;
    }

    RaycastHit2D[] hits;

    //[Header("판정 크기")]
    //public Vector2 size;

    //[Header("판정 범위")]
    //public float maxDistance = 4f;

    //[Header("지속 시간")]
    //public float activeTime;

    public bool hit = false;

    public int machineLayerMask;



    public override void Init()
    {
        if (player.lightningActiveTime <= 0f)
        {
            player.lightningActiveTime = 2f;
        }
        machineLayerMask = (1 << LayerMask.NameToLayer("Machine"));
    }
    public override void Execute()
    {
        if (InputManager.Instance.buttonSkillLightning.wasPressedThisFrame
            && player.isOtherSkillUse() == false
            && player.playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT)
        {
            if (player.LightningCoroutine == null)
            {
                player.LightningCoroutine = ProcessLightning();
                player.StartCoroutine(player.LightningCoroutine);
            }
        }
    }

    public override void PhysicsExecute()
    {
    }

    IEnumerator ProcessLightning()
    {

        player.isSkillUse_Lightning = true;
        var timer = 0f;
        while (timer < player.lightningActiveTime)
        {
            timer += Time.deltaTime;


            yield return new WaitForFixedUpdate();
        }

        hits = Physics2D.CircleCastAll(player.lightningPosition.position, player.lightningRadius, Vector2.up, 0.1f, machineLayerMask);
        //(player.lightningPosition.position, player.waterSize, 0f, player.waterDirection, player.waterDistance, machineLayerMask);

        hit = false;

        foreach (var item in hits)
        {
            if (item.normal.y <= 0f) //반원
            {
                Debug.DrawRay(player.lightningPosition.position, item.collider.gameObject.transform.position, Color.red, 1f);
                var test = item.collider.GetComponent<ElectricableObject>();
                if (test != null)
                {
                    test.OnLightining();
                }
                if (item && hit == false)
                {
                    hit = true;
                }

            }

        }
        yield return null;

        Debug.Log("End L");
        player.isSkillUse_Lightning = false;
        player.LightningCoroutine = null;
    }

}
