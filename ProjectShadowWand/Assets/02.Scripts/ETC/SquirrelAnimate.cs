using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAnimate : MonoBehaviour
{

    public Animator animator;

    private int animatorLookBool;
    private void Awake()
    {
        animatorLookBool = Animator.StringToHash("Look");
    }

    private void Start()
    {
        StartCoroutine(ProcessPlayerMoveCheck());
    }


    private IEnumerator ProcessPlayerMoveCheck()
    {

        //���� ���� �� ���� ��ٸ�
        while (true)
        {
            if (PlayerController.Instance.isGrounded == true)
            {
                break;
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;

        }

        var startPos = PlayerController.Instance.transform.position;
        var playerPos = PlayerController.Instance.transform;

        //x�� ��ġ �̵��� �� ������ ��ٸ�
        while (true)
        {
            if (playerPos.position.x != startPos.x)
            {
                break;
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        yield return new WaitForSeconds(0.2f);
        animator.SetBool(animatorLookBool,true);
    }
}
