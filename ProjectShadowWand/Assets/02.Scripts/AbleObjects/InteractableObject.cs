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
    /// �÷��̾��� ���ͷ�Ʈ������Ʈ�� _b�� ���� �����մϴ�. �׸��� canInterct _b = false�� �� �÷��̾��� tio�� �ڱ� �ڽ��� ������ �ʴٸ�, �ƹ��͵� ���� �ʽ��ϴ�.
    /// </summary>
    /// <param name="_b">true : this, false : null;  </param>
    public virtual void SetTouchedObject(bool _b)
    {
        if (_b)
        {
            player.touchedInteractObject = this;
            player.existInteractObject = true;
        }
        else
        {
            if (player.touchedInteractObject == this)
            {
                player.touchedInteractObject = null;
                player.existInteractObject = false;
            }
        }

    }

    /// <summary>
    /// ��ȣ�ۿ��� �մϴ�.
    /// </summary>
    public virtual void DoInteract() { }

}