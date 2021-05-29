using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    [Header("¿¬°áµÈ ÇÃ·§Æû")]
    public MovePlatform[] movePlatforms;

    public eLiftState destination;

    public LeverGroup leverGroup;
    public int leverIndex;
    public bool isOn;

    public int count;

    public void SetLeverGroupAndIndex(LeverGroup _g, int _i)
    {
        leverGroup = _g;
        leverIndex = _i;
    }
    private void Awake()
    {
        count = movePlatforms.Length;
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        player = PlayerController.Instance;
    }


    public void SetIsOn(bool _b)
    {
        isOn = _b;
    }

    private void SetPlatformsMoving(bool _b)
    {
        if (_b)
        {
            for (int i = 0; i < count; i++)
            {
                movePlatforms[i].canMoving = _b;
                movePlatforms[i].currentDestination = destination;
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                movePlatforms[i].canMoving = _b;
            }
        }

    }
    public override void DoInteract()
    {
        SetIsOn(!isOn);

        if (isOn)
        {
            leverGroup.UpdateLeverToggle(leverIndex);
            SetPlatformsMoving(true);
        }
        else
        {

            SetPlatformsMoving(false);
        }
    }

    public override void SetTouchedObject(bool _b)
    {
        base.SetTouchedObject(_b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(false);
        }

    }

}
