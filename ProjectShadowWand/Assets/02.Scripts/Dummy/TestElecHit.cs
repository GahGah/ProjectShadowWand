using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElecHit : MonoBehaviour
{

    RaycastHit2D[] hits;
    public float maxDistance = 4f;
    public bool hit = false;
    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0f, Vector2.right, maxDistance);

        hit = false;
        foreach (var item in hits)
        {
            if (item)
            {
                hit = true;
                break;
            }

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        //Check if there has been a hit yet
        if (hit)
        {
            foreach (var item in hits)
            {
                if (item)
                {
                    //Draw a Ray forward from GameObject toward the hit
                    Gizmos.DrawRay(transform.position, Vector2.right * item.distance);
                    //Draw a cube that extends to where the hit exists
                    Gizmos.DrawWireCube(transform.position + (Vector3)Vector2.right * item.distance, transform.localScale);

                }
            }
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector2.right * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + (Vector3)Vector2.right * maxDistance, transform.localScale);
        }
    }
}
