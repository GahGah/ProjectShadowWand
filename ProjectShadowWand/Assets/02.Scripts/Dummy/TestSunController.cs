using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSunController : MonoBehaviour
{
   public Transform target;
    public float speed;
    private void Start()
    {
        speed = 0.1f;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
           transform.position += new Vector3(-speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
           transform.position+= new Vector3(speed, 0, 0);

        }

        Vector2 direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        //천천히 움직일때(Slerp)사용
        //Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 1f * Time.deltaTime);

        //하지만 난 바로바로 움직이게 하고 싶기 때문에
        Quaternion rotation = angleAxis;
        transform.rotation = rotation;

    }
}
