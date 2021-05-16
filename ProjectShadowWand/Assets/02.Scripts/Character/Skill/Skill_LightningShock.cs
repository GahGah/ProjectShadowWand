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

    [Tooltip("번개 스킬 발동 이펙트")]
    public GameObject lightningEffect_Shock;

    [Tooltip("번개 스킬 잔류 스파크. 되도록이면 프리팹 파일이어야 합니다.")]
    public GameObject lightningEffect_Spark;
    private GameObject lightningEffect_Spark_Clone;

    public Transform lightningPosition;

    private Transform shockTransform;
    private Transform sparkTransform;



    public override void Init()
    {
        if (player.lightningActiveTime <= 0f)
        {
            player.lightningActiveTime = 2f;
        }

        lightningEffect_Spark_Clone = GameObject.Instantiate(lightningEffect_Spark, lightningPosition.position, Quaternion.identity, null);
        lightningEffect_Spark_Clone.SetActive(false);
        shockTransform = lightningEffect_Shock.transform;
        sparkTransform = lightningEffect_Spark_Clone.transform;

        machineLayerMask = (1 << LayerMask.NameToLayer("Machine"));
    }
    public override void Execute()
    {
        if (InputManager.Instance.buttonSkillLightning.wasPressedThisFrame
            && player.isOtherSkillUse() == false
            && player.playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT
            && player.CanMove())
        {
            if (player.LightningCoroutine == null)
            {
                player.LightningCoroutine = ProcessLightning();
                player.StartCoroutine(player.LightningCoroutine);
            }
            else
            {
                player.StopCoroutine(player.LightningCoroutine);
                player.LightningCoroutine = null;
                player.LightningCoroutine = ProcessLightning();
                player.StartCoroutine(player.LightningCoroutine);

            }
        }
    }

    public override void PhysicsExecute()
    {
    }
    private void FlipShockEffect()
    {
        if (player.isRight == true)
        {
            shockTransform.localScale = new Vector3(Mathf.Abs(shockTransform.localScale.x), shockTransform.localScale.y, shockTransform.localScale.z);
        }
        else
        {

            shockTransform.localScale = new Vector3(Mathf.Abs(shockTransform.localScale.x) * -1f, shockTransform.localScale.y, shockTransform.localScale.z);
        }
    }
    /// <summary>
    /// 번개이펙트의 위치를 설정합니다.
    /// </summary>
    private void InitLightningPosition()
    {
        shockTransform.position = lightningPosition.position;
        sparkTransform.position = lightningPosition.position;
    }
    private IEnumerator ProcessLightning()
    {

        Debug.LogWarning("Start Lightning");

        lightningEffect_Shock.SetActive(false);
        lightningEffect_Spark_Clone.SetActive(false);

        player.isSkillUse_Lightning = true;
        //var timer = 0f;
        //while (timer < player.lightningActiveTime)
        //{
        //    timer += Time.deltaTime;


        //    yield return YieldInstructionCache.WaitForFixedUpdate;
        //}

        yield return new WaitForSeconds(0.4f);
        //해당 시간만큼 기다린 뒤에

        //번개 판정
        InitLightningPosition();
        FlipShockEffect();

        lightningEffect_Shock.SetActive(true);
        lightningEffect_Spark_Clone.SetActive(true);

        yield return new WaitForSeconds(0.1f);

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

        yield return YieldInstructionCache.WaitForEndOfFrame;
        yield return new WaitForSeconds(0.5f);
        player.isSkillUse_Lightning = false;

        yield return new WaitForSeconds(1.5f);
        lightningEffect_Shock.SetActive(false);
        lightningEffect_Spark_Clone.SetActive(false);

        Debug.Log("End Lightning");
        player.LightningCoroutine = null;

    }

}
