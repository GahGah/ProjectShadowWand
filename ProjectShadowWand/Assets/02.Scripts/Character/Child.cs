using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




[RequireComponent(typeof(Rigidbody2D))] //리지드바디 2D 자동추가
public class Child : Character
{

    [Tooltip("[임시] : 반딧불이 된 아이의 위치. 0~2")]
    public int childVal;
    public GameObject choiceCanvas;
    public bool isChoice;
    public PlayerController player;
    public float choiceDistance = 3f;
    public bool isSelectEnd;

    [Header("반딧불~")]
    public Sprite bandit;

    public eChildOption childOption;

    private BoxCollider2D childCollider;
    private void Start()
    {
        choiceCanvas.SetActive(false);
        isSelectEnd = false;
        isChoice = false;
        childOption = eChildOption.TAIN;

        spriteRenderer = GetComponent<SpriteRenderer>();
        childCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (isSelectEnd==false)
        {
            if (isChoice == true)
            {
                return;
            }
            else
            {
                if (isSelectEnd == false && (Vector2.Distance(player.gameObject.transform.position, transform.position) <= choiceDistance))
                {

                    if (InputManager.Instance.keyboard.enterKey.wasPressedThisFrame)
                    {
                        isChoice = true;
                        choiceCanvas.SetActive(true);
                    }
                }
            }

        }
        else //선택이 끝난 상태라면
        {
            if (childOption==eChildOption.TAIN)
            {

            }
            else if (childOption==eChildOption.FRIEND)
            {
                //gameObject.transform.position = player.childPostion[childVal].position;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, choiceDistance);
    }

    public void GoFriendSelected(bool _b)
    {
        if (_b == true)//따라와~
        {
            childOption = eChildOption.FRIEND;
            spriteRenderer.sprite = bandit;
            childCollider.enabled = false;
            transform.localScale = new Vector3(2, 2);
        }
        else
        {
            childOption = eChildOption.TAIN;
            spriteRenderer.color = Color.red;
        }

        isSelectEnd = true;

        choiceCanvas.SetActive(false);
    }
}
