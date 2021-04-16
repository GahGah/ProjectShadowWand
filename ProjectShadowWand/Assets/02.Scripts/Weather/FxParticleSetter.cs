using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FxParticleSetter : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = this.GetComponentsInChildren<ParticleSystem>();
    }

    public void SetRateOverTimeMultiplier(float tm)
    {
        for(int i =0; i< particleSystems.Length; ++i)
        {
            var em = particleSystems[i].emission;
            em.rateOverTimeMultiplier = tm;
        }
    }
}
