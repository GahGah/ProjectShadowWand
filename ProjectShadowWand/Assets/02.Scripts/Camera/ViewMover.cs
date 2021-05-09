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
    [Range(0f, 0.05f)]
    public float moveValue;

    private new Renderer renderer;
    //private float offset;

    private float prevTargetX;

    private string mainTex;

    //private Transform _moverTransform;
    //public Transform moverTransform
    //{
    //    get
    //    {
    //        if (object.ReferenceEquals(_moverTransform, null)) //����ȭ�� ���� ���۷������� ���
    //        {
    //            _moverTransform = transform;
    //        }
    //        return _moverTransform;
    //    }
    //}

    private Transform moverTransform;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        renderer = GetComponent<Renderer>();
        if (target == null) //null�� ���
        {
            target = CameraManager.Instance.currentCamera.transform;
        }

        moverTransform = gameObject.transform;
        prevTargetX = target.transform.position.x;
        mainTex = "_MainTex";

    }
    private void Update()
    {
        moverTransform.position = new Vector2(target.position.x, moverTransform.position.y);
        if (prevTargetX != target.position.x)
        {
            renderer.material.SetTextureOffset(mainTex, new Vector2(target.position.x * moveValue, 0f));
        }

    }
    private void LateUpdate()
    {
        prevTargetX = target.position.x;
    }

}
