using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPlayerConflict : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, this.gameObject.GetComponent<Collider2D>());
    }
}
