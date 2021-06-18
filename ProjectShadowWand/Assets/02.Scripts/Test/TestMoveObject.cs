using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveObject : MonoBehaviour
{
    public TextMesh tm = null;
    public TextMesh tm_pos = null;
    public Vector2 maxVelocity = Vector2.zero;
    public Vector2 maxDistance = Vector2.zero;
    
    public Rigidbody2D rb = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestMove());
    }
    private IEnumerator UpdateMax()
    {
        while (true)
        {

            if (maxVelocity.x < rb.velocity.x)
            {
                maxVelocity.x = rb.velocity.x;
            }
            if (maxVelocity.y < rb.velocity.y)
            {
                maxVelocity.y = rb.velocity.y;
            }


            if (maxDistance.x < rb.position.x)
            {
                maxDistance.x = rb.position.x;
            }

            if (maxDistance.y < rb.position.y)
            {
                maxDistance.y = rb.position.y;
            }

            tm.text = maxVelocity.x.ToString() + "," + maxVelocity.y.ToString();
            tm_pos.text = maxDistance.x.ToString() + "," + maxDistance.y.ToString();

            yield return null;
        }
    }
    private void SetVelocity(Vector2 _velo)
    {
        rb.velocity = _velo;
    }
    private void AddVelocity(Vector2 _velo)
    {
        rb.AddForce(_velo, ForceMode2D.Impulse);
    }

    private Vector2 testVector = new Vector2(6f, 8f);
    private IEnumerator TestMove()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdateMax());

        AddVelocity(testVector);

        var tempVector = new Vector2(rb.velocity.x, rb.velocity.y);
        SetVelocity(tempVector);

        AddVelocity(testVector);

        tempVector = new Vector2(rb.velocity.x, rb.velocity.y);
        SetVelocity(tempVector);

    }
}
