using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    public eFireDirection fireDirection;
    private RaycastHit2D[] hits;

    [Tooltip("불이 번지는 사정거리 입니다.")]
    public float hitDistance;

    [Tooltip("불이 번지는 속도(초)입니다. ")]
    public float spreadSpeed;

    [Tooltip("불이 번질 수 있는가?")]
    public bool canSpread;

    public void Start()
    {
        Init();
    }
    // 0 : 상
    // 1 : 하
    // 2 : 좌
    // 3 : 우
    public void Init()
    {
        InitHits();
        //0은 오바
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

            //spreadSpeed동안 기다린다.
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
        UpdateHits(fireDirection); //directionMode(?)에 따라서 hit 방향이 달라짐
    }
    private void UpdateHits(eFireDirection _dir)
    {
        switch (_dir)
        {
            case eFireDirection.oneDirection:

                break;
            case eFireDirection.twoDirection:

                //좌
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left * hitDistance);

                //우
                hits[3] = Physics2D.Raycast(transform.position, Vector2.right * hitDistance);

                Debug.DrawRay(transform.position, Vector2.right * hitDistance, Color.red);
                Debug.DrawRay(transform.position, Vector2.left * hitDistance, Color.red);


                break;
            case eFireDirection.fourDirection:

                //상
                hits[0] = Physics2D.Raycast(transform.position, Vector2.up * hitDistance);

                //하
                hits[1] = Physics2D.Raycast(transform.position, Vector2.down * hitDistance);

                //좌
                hits[2] = Physics2D.Raycast(transform.position, Vector2.left * hitDistance);

                //우
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
