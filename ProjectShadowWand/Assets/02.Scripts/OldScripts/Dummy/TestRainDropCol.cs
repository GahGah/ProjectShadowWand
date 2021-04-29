using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRainDropCol : MonoBehaviour
{

    public LayerMask layerMask;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public Rigidbody2D rb;

    //void Start()
    //{
    //    collisionEvents = new List<ParticleCollisionEvent>();
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void Update()
    //{
    //    if (rb.IsTouchingLayers(layerMask))
    //    {
    //        Debug.Log("´ê¾Ò¾î!!");
    //    }
    //}
    //void OnParticleCollision(GameObject other)
    //{
    //    Debug.Log(other.name);
    //}
}
