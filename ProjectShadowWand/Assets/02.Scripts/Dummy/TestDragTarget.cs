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
		// ���콺 �������� ���
		var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			// ù��° �ݶ��̴��� ������
			// ���� ���� �ݶ��̴��� ���ؼ��� �����ϴٴµ�...��...
			var collider = Physics2D.OverlapPoint(worldPos, dragLayers);
			if (!collider)
				return;

			// �ٵ� = collider�� ������ٵ� �޸��� ������
			var body = collider.attachedRigidbody;
			if (!body)
				return;

			//������ٵ� 2D �޸� ������Ʈ�� Ÿ������Ʈ2D Add
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

		// ������Ʈ Ÿ��
		if (targetJoint)
		{
			targetJoint.target = worldPos;

			// Ÿ���̶� ����Ʈ ���̿� ���� �׸�
			if (drawDragLine)
				Debug.DrawLine(targetJoint.transform.TransformPoint(targetJoint.anchor), worldPos, Color);
		}
	}
}
