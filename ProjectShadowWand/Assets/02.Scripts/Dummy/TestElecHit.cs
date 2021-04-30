using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElecHit : MonoBehaviour
{

    RaycastHit2D[] hits;
    [Header("판정 크기")]
    public Vector2 size;

    [Header("판정 범위")]
    public float maxDistance = 4f;
    public bool hit = false;
    public LayerMask machineLayerMask;
    // Update is called once per frame
    void Update()
    {
        hits = Physics2D.BoxCastAll(transform.position, size, 0f, Vector2.right, maxDistance, machineLayerMask);

        hit = false;
        foreach (var item in hits)
        {
            var test = item.collider.GetComponent<ElectricableObject>();
            if (test != null)
            {
                //if (test is IElectricable) 이거 트루 나옴 ㅇㅇ
                //{
                //}
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
                    Gizmos.DrawWireCube(transform.position + (Vector3)Vector2.right * item.distance, size);

                }
            }
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(transform.position, size);
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector2.right * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + (Vector3)Vector2.right * maxDistance, size);
        }
    }
}
