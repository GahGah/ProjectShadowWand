using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFire : MonoBehaviour
{
    private RaycastHit2D[] hit;
    private int lm;

    public bool isFirst;
    public GameObject gm;
    private void Awake()
    {
        hit = new RaycastHit2D[1];
        lm = ((1 << LayerMask.NameToLayer("Fire")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));
        lm = ~lm;
    }
    // Start is called before the first frame update
    void Start()
    {
        isFirst = true;
        hit = new RaycastHit2D[1];
        gm = this.gameObject;
        StartCoroutine(ProcessFire());
    }
    IEnumerator ProcessCreate()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (isFirst)
            {
                isFirst = false;
                GameObject test = Instantiate(gm, new Vector3(transform.position.x + 5f, transform.position.y), transform.rotation);
            }

        }
    }
    IEnumerator ProcessFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (isFirst)
            {
                isFirst = false;


                Debug.LogError("Wait End");

                //오른쪽으로 3만큼 레이캐스트
                hit[0] = Physics2D.Raycast(transform.position, Vector2.right, 3f, lm);

                //닿은게 있다면
                if (hit[0] == true)
                {
                    Debug.Log(hit[0].collider.name);
                    //스프라이트 렌더러를 가져와서
                    var test = hit[0].collider.gameObject.GetComponent<SpriteRenderer>();
                    if (test != null) //가져와졌다면
                    {
                        //레이어를 이그노어로 변경하고
                        test.gameObject.layer = 2;

                        //그 자리에 생성하기
                        var testObject = Instantiate(gameObject, test.transform.position, transform.rotation);
                        Debug.Log("생성");
                    }
                }

            }

        }


    }


}
