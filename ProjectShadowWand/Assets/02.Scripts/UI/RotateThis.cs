using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThis : MonoBehaviour
{
    Transform trans;
    Vector3 rotateZ;

    public bool isStop;

    public void Init()
    {
        isStop = false;
        trans = transform;
        rotateZ = new Vector3(0f, 0f, 5f);
    }

    public IEnumerator ProcessRotate()
    {
        while (!isStop)
        {
            trans.Rotate(rotateZ, -100f * Time.unscaledDeltaTime);
            yield return null;
        }
    }
    void FixedUpdate()
    {

    }
}
