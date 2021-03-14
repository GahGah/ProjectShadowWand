using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{

    public Vector2 currentTopPosition;
    public Vector2 currentNormal;
    public Collider2D tileCollider;

    private void Update()
    {
        currentTopPosition = new Vector2(transform.position.x, tileCollider.bounds.extents.y + tileCollider.offset.y);
        currentNormal = transform.up;
    }
}
