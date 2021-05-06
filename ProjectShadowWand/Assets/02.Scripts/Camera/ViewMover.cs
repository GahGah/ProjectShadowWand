using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶��� �����ӿ� ���缭 ������Ʈ�� offset�� �����ŵ�ϴ�.
/// </summary>
public class ViewMover : MonoBehaviour
{
    public Transform target;
    [Header("�����̴� ����"), Tooltip("ī�޶� ������ ��, �ش� �� ��ŭ �߰��� �� �����Դϴ�. ")]
    [Range(0f, 0.1f)]
    public float moveValue;

    private new Renderer renderer;
    private float offset;

    private float prevTargetX;
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        offset = 0f;
        renderer = GetComponent<Renderer>();
        if (target == null)
        {
            target = CameraManager.Instance.currentCamera.transform;
        }
        prevTargetX = target.transform.position.x;

    }
    private void Update()
    {
        gameObject.transform.position = new Vector2(target.position.x, gameObject.transform.position.y);
        if (prevTargetX != target.position.x)
        {
            renderer.material.SetTextureOffset("_MainTex", new Vector2(target.position.x * moveValue, 0f));
            Debug.Log("���� ������Ʈ");
        }

    }
    private void LateUpdate()
    {
        prevTargetX = target.position.x;
    }

}
