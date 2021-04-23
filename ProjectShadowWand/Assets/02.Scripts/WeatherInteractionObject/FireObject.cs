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

    public void Start()
    {
        Init();
    }
    // 0 : ��
    // 1 : ��
    // 2 : ��
    // 3 : ��
    public void Init()
    {
        InitHits();
        //0�� ����
        if (spreadSpeed == 0)
        {
            spreadSpeed = 5f;
        }
    }
    private void InitHits()
    {
        hits = new RaycastHit2D[4];
    }


    private void FixedUpdate()
    {
    }

    private IEnumerator ProcessFire()
    {
        while (canSpread)
        {
            var timer = 0f;

            //spreadSpeed���� ��ٸ���.
            while (timer < spreadSpeed)
            {
                timer += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            SpreadFire();
        }
    }

    private void SpreadFire()
    {
        UpdateHits(fireDirection); //directionMode(?)�� ���� hit ������ �޶���
    }
    private void UpdateHits(eFireDirection _dir)
    {
        switch (_dir)
        {
            case eFireDirection.oneDirection:

                break;
            case eFireDirection.twoDirection:

                //��
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left * hitDistance);

                //��
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right * hitDistance);

                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red);


                break;
            case eFireDirection.fourDirection:

                //��
                hits[0] = Physics2D.Raycast(transform.position, Vector2.up * hitDistance);

                //��
                hits[1] = Physics2D.Raycast(transform.position, Vector2.down * hitDistance);

                //��
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left * hitDistance);

                //��
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right * hitDistance);



                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.down * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.up * hitDistance, Color.red);
                break;
            default:
                break;
        }

    }
}
