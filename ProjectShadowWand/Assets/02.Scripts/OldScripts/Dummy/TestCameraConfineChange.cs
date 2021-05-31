using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct cameraConfine
{
  public  Vector2 size;
    public Vector2 pos;
}

public class TestCameraConfineChange : MonoBehaviour
{
    //    public Vector2 factorySize;
    //    public Vector2 factoryPos;

    //    cameraConfine originalConfine;

    //    CameraManager cm;

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        cm = CameraManager.Instance;
    //        originalConfine.pos = cm.confinePos;
    //        originalConfine.size = cm.confineSize;

    //    }


    //    void ChangeConfine(Vector2 pos, Vector2 size)
    //    {
    //        cm.confinePos = pos;
    //        cm.confineSize = size;
    //    }
    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        if (collision.CompareTag("Player"))
    //        {
    //            ChangeConfine(factoryPos, factorySize);
    //        }
    //    }

    //    private void OnTriggerExit2D(Collider2D collision)
    //    {
    //        if (collision.CompareTag("Player"))
    //        {
    //            ChangeConfine(originalConfine.pos, originalConfine.size);
    //        }
    //    }

    //    private void OnDrawGizmos()
    //    {
    //            Gizmos.color = Color.blue;
    //        Gizmos.DrawWireCube(factoryPos, factorySize);

    //    }
}
