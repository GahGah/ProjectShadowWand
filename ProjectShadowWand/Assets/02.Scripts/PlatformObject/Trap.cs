using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    CatchableObject currentCatchedObject;

    [Tooltip("아이템을 잡을 때, 고정될 위치입니다. ")]
    public Transform catchedPosition;

    public Collider2D myCollider;

    private bool isCatched;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }
    /// <summary>
    /// 플레이어 들어왔다면 실행하는 함수.
    /// </summary>
    public void EnterPlayer()
    {
        var playerCatchedObject = PlayerController.Instance.GetCatchedObject();
        if (ReferenceEquals(playerCatchedObject, null) == false)//뭘 들고있다면
        {
            currentCatchedObject = playerCatchedObject;
            isCatched = true;
            PlayerController.Instance.PutCatchedObject();
            Debug.Log(currentCatchedObject.name);

            currentCatchedObject.transform.position = catchedPosition.position;
            currentCatchedObject.canCatched = false;
        }
        else //아니라면
        {
            //PlayerController.Instance.GoDie();
        }
    }


    /// <summary>
    /// 트랩을 꽊 ! 닫습니다.
    /// </summary>
    public void CloseTrap()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnterPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isCatched == true)
        {
            myCollider.isTrigger = false;
        }
    }
}
