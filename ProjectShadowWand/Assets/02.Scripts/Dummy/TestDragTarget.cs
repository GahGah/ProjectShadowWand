using UnityEngine;

public class TestDragTarget : MonoBehaviour
{
	public LayerMask dragLayers;

	[Range(0.0f, 100.0f)]
	public float damping = 1.0f;

	[Range(0.0f, 100.0f)]
	public float frequency = 5.0f;

	public bool drawDragLine = true;
	public Color Color = Color.cyan;

	private TargetJoint2D targetJoint;

	void Update()
	{
		// 마우스 포지션을 계산
		var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			// 첫번째 콜라이더를 가져옴
			// 여러 개의 콜라이더에 대해서도 가능하다는데...음...
			var collider = Physics2D.OverlapPoint(worldPos, dragLayers);
			if (!collider)
				return;

			// 바디 = collider에 리지드바디 달린거 가져옴
			var body = collider.attachedRigidbody;
			if (!body)
				return;

			//리지드바디 2D 달린 오브젝트에 타겟조인트2D Add
			targetJoint = body.gameObject.AddComponent<TargetJoint2D>();
			targetJoint.dampingRatio = damping;
			targetJoint.frequency = frequency;

			// Attach the anchor to the local-point where we clicked.
			targetJoint.anchor = targetJoint.transform.InverseTransformPoint(worldPos);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Destroy(targetJoint);
			targetJoint = null;
			return;
		}

		// 업데이트 타겟
		if (targetJoint)
		{
			targetJoint.target = worldPos;

			// 타겟이랑 조인트 사이에 선을 그림
			if (drawDragLine)
				Debug.DrawLine(targetJoint.transform.TransformPoint(targetJoint.anchor), worldPos, Color);
		}
	}
}
