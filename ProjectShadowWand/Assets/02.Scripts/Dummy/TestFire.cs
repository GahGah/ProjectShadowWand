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

                //���������� 3��ŭ ����ĳ��Ʈ
                hit[0] = Physics2D.Raycast(transform.position, Vector2.right, 3f, lm);

                //������ �ִٸ�
                if (hit[0] == true)
                {
                    Debug.Log(hit[0].collider.name);
                    //��������Ʈ �������� �����ͼ�
                    var test = hit[0].collider.gameObject.GetComponent<SpriteRenderer>();
                    if (test != null) //���������ٸ�
                    {
                        //���̾ �̱׳��� �����ϰ�
                        test.gameObject.layer = 2;

                        //�� �ڸ��� �����ϱ�
                        var testObject = Instantiate(gameObject, test.transform.position, transform.rotation);
                        Debug.Log("����");
                    }
                }

            }

        }


    }


}
