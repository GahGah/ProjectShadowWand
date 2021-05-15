using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WaterWave : Skill
{


    RaycastHit2D[] hits;

    public Transform startPos;
    //[Header("판정 크기")]
    //public Vector2 size;

    //[Header("판정 범위")]
    //public float maxDistance = 4f;

    //[Header("지속 시간")]
    //public float activeTime;

    public bool hit = false;

    public int plantLayerMask;

    [Tooltip("물 스킬 시전 이펙트")]
    public GameObject waterEffect_Set;

    [Tooltip("물 스킬 발동 이펙트")]
    public GameObject waterEffect_Splash;
    public Transform waterPosition;

    private float splashCheckDistance;
    private int groundCheckMask;
    private Vector2 rayHitPosition;

    private Transform splashTransform;

    [Tooltip("스플래쉬 위치잡기용 레이캐스트에 쓰이는 시작 위치입니다.")]
    public Transform originalRayPosition;
    public Skill_WaterWave(PlayerController _p)
    {
        player = _p;
        groundCheckMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Ground_Soft")) | (1 << LayerMask.NameToLayer("Machine")) | (1 << LayerMask.NameToLayer("Ground_Hard"));
    }


    public override void Init()
    {



        splashCheckDistance = 2f;
        if (player.waterActiveTime <= 0f)
        {
            player.waterActiveTime = 2f;
        }
        startPos = waterEffect_Splash.transform;
        originalRayPosition = waterEffect_Set.transform;
        splashTransform = waterEffect_Splash.transform;
        // plantLayerMask = LayerMask.NameToLayer("Plant");
        plantLayerMask = (1 << LayerMask.NameToLayer("Plant"));
    }
    public override void Execute()
    {
        UpdateWaterDirection();
        if (InputManager.Instance.buttonSkillWater.wasPressedThisFrame
            && player.isOtherSkillUse() == false
            && player.playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT)
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

    public void SetRayPosition()
    {
        rayHitPosition = new Vector2(originalRayPosition.position.x, originalRayPosition.position.y - splashCheckDistance);
    }
    RaycastHit2D posHit;
    IEnumerator ProcessWater()
    {
        var splashOn = false;

        player.isSkillUse_Water = true;

        yield return new WaitForSeconds(0.7f);

        waterEffect_Set.SetActive(true);
        SetRayPosition();
        //yield return new WaitUntil(() => waterEffect_Set.activeSelf == false);

        yield return new WaitForSeconds(1f);
        //WaterSet가 사라진 이후
        if (splashOn == false)
        {
            splashOn = true;

        }

        posHit = Physics2D.Raycast(rayHitPosition, Vector2.down, 50f, groundCheckMask);
        if (posHit)
        {
            Debug.DrawLine(rayHitPosition, posHit.point, Color.cyan, 1f);

            waterEffect_Splash.transform.position =
                new Vector3(waterEffect_Splash.transform.position.x, posHit.point.y, waterEffect_Splash.transform.position.z);

            if (player.isRight == true)
            {
                splashTransform.localScale = new Vector2(Mathf.Abs(splashTransform.localScale.x), splashTransform.localScale.y);
            }
            else
            {

                splashTransform.localScale = new Vector2(Mathf.Abs(splashTransform.localScale.x) * -1f, splashTransform.localScale.y);
            }

        }
        yield return YieldInstructionCache.WaitForFixedUpdate;

        waterEffect_Splash.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        //WaterSplash가 적당한 모습일때
        Vector2 pos = startPos.transform.position;
        hits = Physics2D.BoxCastAll(pos, player.waterSize, 0f, player.waterDirection, player.waterDistance, plantLayerMask);

        hit = false;

        foreach (var item in hits)
        {
            GrowableObject growableObject = item.collider.GetComponent<PlantSeed>();

            if (growableObject != null)
            {
                Debug.Log(growableObject.name);
                growableObject.OnWater(); //젖음 판정
                growableObject.StartGrow(); //자람 시작
            }
            if (item && hit == false)
            {
                hit = true;
            }

        }
        yield return YieldInstructionCache.WaitForEndOfFrame;

        yield return new WaitForSeconds(0.3f);
        //WaterSplash가 끝나고

        waterEffect_Splash.SetActive(false);
        //   Debug.Log("End Water");
        player.isSkillUse_Water = false;
        player.WaterCoroutine = null;
        yield break;
    }

    //While문 쓰는 버전(안될수도)
    //IEnumerator ProcessWater()
    //{
    //    var splashOn = false;
    //    Debug.Log("StartWater");
    //    player.isSkillUse_Water = true;

    //    yield return new WaitForSeconds(0.7f);

    //    waterEffect_Set.SetActive(true);

    //    var timer = 0f;

    //    while (timer < player.waterActiveTime)
    //    {
    //        timer += Time.deltaTime;

    //        hits = Physics2D.BoxCastAll(startPos.transform.position, player.waterSize, 0f, player.waterDirection, player.waterDistance, plantLayerMask);

    //        if (waterEffect_Set.activeSelf == false && splashOn ==false)
    //        {
    //            splashOn = true;
    //            waterEffect_Splash.SetActive(true);
    //        }

    //        hit = false;

    //        foreach (var item in hits)
    //        {
    //            var test = item.collider.GetComponent<GrowableObject>();
    //            if (test != null)
    //            {
    //                Debug.Log(test.name);
    //                test.OnWater();
    //            }
    //            if (item && hit == false)
    //            {
    //                hit = true;
    //            }

    //        }
    //        yield return YieldInstructionCache.WaitForFixedUpdate;
    //    }
    //    waterEffect_Splash.SetActive(false);
    //    Debug.Log("End Water");
    //    player.isSkillUse_Water = false;
    //    player.WaterCoroutine = null;
    //}

}
