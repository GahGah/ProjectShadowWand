using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected PlayerController player;

    protected string tagPlayer;
    public virtual void Init()
    {
        tagPlayer = "Player";
    }



    /// <summary>
    /// 플레이어의 터치인터랙트오브젝트를 _b에 따라 변경합니다. _b = false일 때 플레이어의 tio에 자기 자신이 들어가있지 않다면, 아무것도 하지 않습니다.
    /// </summary>
    /// <param name="_b">true : this, false : null;  </param>
    public virtual void SetTouchedObject(bool _b)
    {
        if (_b)
        {
            player.touchedInteractObject = this;
        }
        else
        {
            if (player.touchedInteractObject == this)
            {
                player.touchedInteractObject = null;
            }
        }

    }

    /// <summary>
    /// 상호작용을 합니다.
    /// </summary>
    public virtual void DoInteract() { }

}