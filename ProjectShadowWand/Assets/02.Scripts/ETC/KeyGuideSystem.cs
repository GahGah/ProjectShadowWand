using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGuideSystem : MonoBehaviour
{

    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;

    public GameObject jump;
    public GameObject talk;
    public GameObject deulgi;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTalking());
        StartCoroutine(CheckRightMoving());
    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator CheckTalking()
    {
        talk.SetActive(true);
        while (true)
        {
            if (PlayerController.Instance.isTalking == true)
            {
                break;
            }
            yield return null;
        }

        talk.SetActive(false);
        yield break;
    }
    private IEnumerator CheckRightMoving()
    {
        //∂•ø° ¥Í¿ª ∂ß ±Ó¡ˆ ±‚¥Ÿ∏≤
        while (true)
        {
            if (PlayerController.Instance.isGrounded == true)
            {
                break;
            }

            yield return null;
        }

        right.SetActive(true);
        left.SetActive(true);

        yield return null;

        StartCoroutine(CheckLeftMoving());

        while (true)
        {
            if (InputManager.Instance.buttonMoveRight.wasPressedThisFrame)
            {
                right.SetActive(false);
                break;
            }
            yield return null;
        }

        yield return null;
        StartCoroutine(CheckLadder());

    }

    private IEnumerator CheckLeftMoving()
    {

        while (true)
        {

            if (InputManager.Instance.buttonMoveLeft.wasPressedThisFrame)
            {
                left.SetActive(false);
                break;
            }
            yield return null;
        }

        yield break;

    }


    private IEnumerator CheckJumping()
    {
        jump.SetActive(true);

        while (true)
        {
            if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame)
            {

                break;
            }
            yield return null;
        }

        jump.SetActive(false);

    }
    private IEnumerator CheckLadder()
    {

        up.SetActive(true);
        down.SetActive(true);
        while (true)
        {
            if (PlayerController.Instance.isClimbLadder == true)
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(CheckJumping());
        yield return new WaitForSeconds(0.5f);
        up.SetActive(false);

        down.SetActive(false);


    }



}
