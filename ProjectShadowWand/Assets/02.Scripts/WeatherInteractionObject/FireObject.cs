using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{

    public WindController windConntroller;

    [Header("불의 방향")]
    public eFireDirection fireDirection;
    private RaycastHit2D[] hits;

    [Tooltip("불이 번지는 사정거리 입니다.")]
    public float hitDistance;

    [Tooltip("불이 번지는 속도(초)입니다. ")]
    public float spreadSpeed;

    [Tooltip("불이 번질 수 있는가?")]
    public bool canSpread;

    /// <summary>
    /// 업데이트문에서 코루틴을 돌리기 위해
    /// </summary>
    public bool isStartSpread = false;

    //public GameObject fireObject;
    private BurnableObject burnableObject;

    [HideInInspector] public int hitMask;

    [HideInInspector]
    public eFireDirection currentFireDirection;

    public GameObject smokeObject;
    private void Awake()
    {
        Init();
        // Debug.Log("AWake");
    }
    // 0 : 상
    // 1 : 하
    // 2 : 좌
    // 3 : 우
    public void Init()
    {
        InitHits();
        isStartSpread = false;
        //0은 오바
        if (spreadSpeed == 0)
        {
            spreadSpeed = 5f;
        }
        currentFireDirection = fireDirection;
    }
    private void Start()
    {
        StartCoroutine(ProcessFire());
    }

    private void InitHits()
    {
        hits = new RaycastHit2D[4];

        hitMask = ((1 << LayerMask.NameToLayer("Fire")) 
            | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ignore Raycast")) 
            | (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("FlamingObject")));
        hitMask = ~hitMask;
    }


    private void FixedUpdate()
    {
        if (isStartSpread == false)
        {
            isStartSpread = true;

        }
    }

    private IEnumerator ProcessFire()
    {
        //  Debug.Log("Start Fire!!");
        while (canSpread)
        {
            var timer = 0f;

            //spreadSpeed동안 기다린다.
            while (timer < spreadSpeed)
            {
                timer += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            CheckSpreadFire();

            yield return null;
        }
    }

    private void CheckSpreadFire()
    {

        UpdateHits(currentFireDirection); //directionMode(?)에 따라서 hit 방향이 달라짐

        switch (currentFireDirection)
        {
            case eFireDirection.oneDirection:
                if (windConntroller.windDirection == eWindDirection.LEFT)
                {
                    if (hits[2] == true)
                    {
                        var _burnObject = GetBurnable(2);

                        if (_burnObject != null) //제대로 가져와졌다면
                        {
                            GoSpread(_burnObject);
                        }
                    }

                }
                else if (windConntroller.windDirection == eWindDirection.RIGHT)
                {
                    if (hits[3] == true)
                    {
                        var _burnObject = GetBurnable(3);

                        if (_burnObject != null) //제대로 가져와졌다면
                        {
                            GoSpread(_burnObject);
                        }

                    }
                }
                else //NONE
                {

                }

                //어렵다...
                break;

            case eFireDirection.twoDirection:

                // UpdateHits(fireDirection); //directionMode(?)에 따라서 hit 방향이 달라짐
                if (hits[2] == true)//뭔가 닿았다면
                {
                    // Debug.Log("닿음");
                    var _burnObject = GetBurnable(2);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[3] == true)//뭔가 닿았다면
                {
                    var _burnObject = GetBurnable(3);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }
                break;

            case eFireDirection.fourDirection:
                if (hits[0] == true)//뭔가 닿았다면
                {
                    var _burnObject = GetBurnable(0);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[1] == true)//뭔가 닿았다면
                {
                    var _burnObject = GetBurnable(1);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }
                if (hits[2] == true)//뭔가 닿았다면
                {
                    var _burnObject = GetBurnable(2);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[3] == true)//뭔가 닿았다면
                {
                    var _burnObject = GetBurnable(3);

                    if (_burnObject != null) //제대로 가져와졌다면
                    {
                        GoSpread(_burnObject);
                    }
                }
                break;

            default:
                break;
        }

    }

    private BurnableObject GetBurnable(int _hitsIndex)
    {

        return hits[_hitsIndex].collider.GetComponent<BurnableObject>();
    }

    private void GoSpread(BurnableObject _bo)
    {
        if (_bo.burnType == eBurnableType.BURN) //불이 붙을 수 있는 오브젝트일 경우
        {
            if (_bo.canBurn && _bo.isBurning == false) //탈 수 있으며, 이미 타고 있는 오브젝트가 아닌 것만 가능
            {
                _bo.fireObject = Instantiate(gameObject, _bo.transform.position, Quaternion.identity, null).GetComponent<FireObject>();
                _bo.fireObject.burnableObject = _bo;

                _bo.gameObject.layer = LayerMask.NameToLayer("FlamingObject");

                _bo.fireObject.isStartSpread = false;
                _bo.fireObject.name = "FireObject";

                _bo.isBurning = true; //타는 중
                _bo.isBurned = true; // 탄 적 있음
            }
        }
        else if (_bo.burnType == eBurnableType.SMOKE) //연기만 붙을 수 있는 오브젝트일 경우
        {
            if (_bo.canBurn && _bo.isBurning == false)
            {
                
            }
        }

    }
    private void UpdateHits(eFireDirection _dir)
    {
        switch (_dir)
        {
            case eFireDirection.oneDirection:
                if (windConntroller.windDirection == eWindDirection.LEFT)
                {                //좌
                    hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);
                    Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red, 1f);
                }
                else if (windConntroller.windDirection == eWindDirection.RIGHT)
                {
                    //우
                    hits[3] = Physics2D.Raycast(transform.position, Vector2.right, hitDistance, hitMask);

                    Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red, 1f);
                }
                else
                {
                    currentFireDirection = fireDirection;
                    UpdateHits(currentFireDirection);
                }
                break;

            case eFireDirection.twoDirection:

                //좌
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);

                //우
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right, hitDistance, hitMask);


                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red, 1f);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red, 1f);

                break;
            case eFireDirection.fourDirection:

                //상
                hits[0] = Physics2D.Raycast(transform.position, Vector2.up, hitDistance, hitMask);

                //하
                hits[1] = Physics2D.Raycast(transform.position, Vector2.down, hitDistance, hitMask);

                //좌
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);

                //우
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right, hitDistance, hitMask);



                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.down * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.up * hitDistance, Color.red);
                break;
            default:
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.CompareTag("Water"))
        //{

        //}

        if (collision.gameObject == windConntroller.gameObject)
        {
            currentFireDirection = eFireDirection.oneDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("WindController"))
        {
            //Debug.Log("test");

            currentFireDirection = eFireDirection.oneDirection;
        }
    }

    /// <summary>
    /// 형식상 불 오브젝트를 사라지게 합니다. 데스트로이 일수도 있고...그렇습니다. 
    /// </summary>
    public void DestroyFireObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
