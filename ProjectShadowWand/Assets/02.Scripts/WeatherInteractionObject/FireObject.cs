using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
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

    public GameObject fireObject;

    public int hitMask;
    private void Awake()
    {
        Init();
        Debug.Log("AWake");
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
    }
    private void Start()
    {
        StartCoroutine(ProcessFire());
    }

    private void InitHits()
    {
        hits = new RaycastHit2D[4];

        hitMask = ((1 << LayerMask.NameToLayer("Fire")) | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));
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
        Debug.Log("Start Fire!!");
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

        UpdateHits(fireDirection); //directionMode(?)�� ���� hit ������ �޶���

        switch (fireDirection)
        {
            case eFireDirection.oneDirection:
                //��ƴ�...
                break;

            case eFireDirection.twoDirection:

               // UpdateHits(fireDirection); //directionMode(?)�� ���� hit ������ �޶���
                if (hits[2] == true)//���� ��Ҵٸ�
                {
                    Debug.Log("����");
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
        Debug.Log("Go Spread!! : " + _bo.name);
        if (_bo.canBurn && _bo.isBurning == false) //Ż �� ������, �̹� Ÿ�� �ִ� ������Ʈ�� �ƴ� �͸� ����
        {
            _bo.fireObject = Instantiate(gameObject, _bo.transform.position, Quaternion.identity, null).GetComponent<FireObject>();

            _bo.gameObject.layer = 2;
            _bo.gameObject.GetComponent<SpriteRenderer>().color = Color.black;

            _bo.fireObject.isStartSpread = false;
            _bo.fireObject.name = "FireObject";
            
            _bo.isBurning = true; //Ÿ�� ��
            _bo.isBurned = true; // ź �� ����

        }
    }
    private void UpdateHits(eFireDirection _dir)
    {
        switch (_dir)
        {
            case eFireDirection.oneDirection:

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
                hits[0] = Physics2D.Raycast(transform.position, Vector2.up , hitDistance, hitMask);

                //��
                hits[1] = Physics2D.Raycast(transform.position, Vector2.down , hitDistance, hitMask);

                //��
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left , hitDistance, hitMask);

                //��
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right , hitDistance, hitMask);



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
        Debug.Log(collision.name);
        if (collision.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }

    }
}
