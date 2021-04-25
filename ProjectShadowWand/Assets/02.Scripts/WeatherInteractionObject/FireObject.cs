using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{

    public WindController windConntroller;

    [Header("���� ����")]
    public eFireDirection fireDirection;
    private RaycastHit2D[] hits;

    [Tooltip("���� ������ �����Ÿ� �Դϴ�.")]
    public float hitDistance;

    [Tooltip("���� ������ �ӵ�(��)�Դϴ�. ")]
    public float spreadSpeed;

    [Tooltip("���� ���� �� �ִ°�?")]
    public bool canSpread;

    /// <summary>
    /// ������Ʈ������ �ڷ�ƾ�� ������ ����
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
    // 0 : ��
    // 1 : ��
    // 2 : ��
    // 3 : ��
    public void Init()
    {
        InitHits();
        isStartSpread = false;
        //0�� ����
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

            //spreadSpeed���� ��ٸ���.
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

        UpdateHits(currentFireDirection); //directionMode(?)�� ���� hit ������ �޶���

        switch (currentFireDirection)
        {
            case eFireDirection.oneDirection:
                if (windConntroller.windDirection == eWindDirection.LEFT)
                {
                    if (hits[2] == true)
                    {
                        var _burnObject = GetBurnable(2);

                        if (_burnObject != null) //����� ���������ٸ�
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

                        if (_burnObject != null) //����� ���������ٸ�
                        {
                            GoSpread(_burnObject);
                        }

                    }
                }
                else //NONE
                {

                }

                //��ƴ�...
                break;

            case eFireDirection.twoDirection:

                // UpdateHits(fireDirection); //directionMode(?)�� ���� hit ������ �޶���
                if (hits[2] == true)//���� ��Ҵٸ�
                {
                    // Debug.Log("����");
                    var _burnObject = GetBurnable(2);

                    if (_burnObject != null) //����� ���������ٸ�
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[3] == true)//���� ��Ҵٸ�
                {
                    var _burnObject = GetBurnable(3);

                    if (_burnObject != null) //����� ���������ٸ�
                    {
                        GoSpread(_burnObject);
                    }
                }
                break;

            case eFireDirection.fourDirection:
                if (hits[0] == true)//���� ��Ҵٸ�
                {
                    var _burnObject = GetBurnable(0);

                    if (_burnObject != null) //����� ���������ٸ�
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[1] == true)//���� ��Ҵٸ�
                {
                    var _burnObject = GetBurnable(1);

                    if (_burnObject != null) //����� ���������ٸ�
                    {
                        GoSpread(_burnObject);
                    }
                }
                if (hits[2] == true)//���� ��Ҵٸ�
                {
                    var _burnObject = GetBurnable(2);

                    if (_burnObject != null) //����� ���������ٸ�
                    {
                        GoSpread(_burnObject);
                    }
                }

                if (hits[3] == true)//���� ��Ҵٸ�
                {
                    var _burnObject = GetBurnable(3);

                    if (_burnObject != null) //����� ���������ٸ�
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
        if (_bo.burnType == eBurnableType.BURN) //���� ���� �� �ִ� ������Ʈ�� ���
        {
            if (_bo.canBurn && _bo.isBurning == false) //Ż �� ������, �̹� Ÿ�� �ִ� ������Ʈ�� �ƴ� �͸� ����
            {
                _bo.fireObject = Instantiate(gameObject, _bo.transform.position, Quaternion.identity, null).GetComponent<FireObject>();
                _bo.fireObject.burnableObject = _bo;

                _bo.gameObject.layer = LayerMask.NameToLayer("FlamingObject");

                _bo.fireObject.isStartSpread = false;
                _bo.fireObject.name = "FireObject";

                _bo.isBurning = true; //Ÿ�� ��
                _bo.isBurned = true; // ź �� ����
            }
        }
        else if (_bo.burnType == eBurnableType.SMOKE) //���⸸ ���� �� �ִ� ������Ʈ�� ���
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
                {                //��
                    hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);
                    Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red, 1f);
                }
                else if (windConntroller.windDirection == eWindDirection.RIGHT)
                {
                    //��
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

                //��
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);

                //��
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right, hitDistance, hitMask);


                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red, 1f);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red, 1f);

                break;
            case eFireDirection.fourDirection:

                //��
                hits[0] = Physics2D.Raycast(transform.position, Vector2.up, hitDistance, hitMask);

                //��
                hits[1] = Physics2D.Raycast(transform.position, Vector2.down, hitDistance, hitMask);

                //��
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left, hitDistance, hitMask);

                //��
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
    /// ���Ļ� �� ������Ʈ�� ������� �մϴ�. ����Ʈ���� �ϼ��� �ְ�...�׷����ϴ�. 
    /// </summary>
    public void DestroyFireObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
