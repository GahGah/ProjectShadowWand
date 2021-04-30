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
            var test = item.collider.GetComponent<ElectricableObject>();
            if (test != null)
            {
                if (test is IElectricable)
                {
                    Debug.Log("일렉트리케이블!!");
                }
            }
            if (item && hit == false)
            {
                hit = true;
            }

        }
    }
    void OnDrawGizmos()
    {


        //Check if there has been a hit yet
        if (hit)
        {
            Gizmos.color = Color.cyan;

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
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(transform.position, transform.localScale);
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector2.right * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + (Vector3)Vector2.right * maxDistance, transform.localScale);
        }
    }
}
