using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNormalHit : MonoBehaviour
{

    public BoxCollider2D col;
    public float distance;

    private RaycastHit2D hit;
    private bool drawGizmos;
    private void Update()
    {
        UpdateGroundCheck_Cast();
    }
    /// <summary>
    /// �ڽ�ĳ��Ʈ�� �� üũ�� �մϴ�.
    /// </summary>
    private void UpdateGroundCheck_Cast()
    {
        var hitSize = new Vector2(col.size.x, 0.1f);
        var castStartPos = new Vector2(col.bounds.center.x, col.bounds.min.y);
        hit = Physics2D.BoxCast(castStartPos, hitSize, 0f, Vector2.down, distance);
        //groundHit = Physics2D.Raycast(castStartPos, Vector2.down, groundCheckDistance, groundCheckMask);
        var groundPerp = Vector2.Perpendicular(hit.normal).normalized; //-1 ���������
        var groundAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (hit == true)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
            if (Mathf.Abs(hit.point.y - castStartPos.y) <= 0.1f && groundAngle != 90f)
            {
                drawGizmos = true;
            }
            else
            {
                drawGizmos = false;
            }
        }
        else
        {
        }

    }

    private void OnDrawGizmos()
    {
        if (hit && drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }

    }
}
