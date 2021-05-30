using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶��� �����ӿ� ���缭 ������Ʈ�� offset�� �����ŵ�ϴ�.
/// </summary>
public class ViewMover : MonoBehaviour
{

    delegate void ProcessMoveDelegate();
    ProcessMoveDelegate ProcessMove;
    public bool moveTex;

    [Header("moveTex üũ"), Tooltip("ī�޶� ������ ��, �ش� �� ��ŭ �߰��� �� �����Դϴ�. ")]
    [Range(0f, 0.05f)]
    public float moveValue_Tex;

    [Header("moveTex üũ X"), Tooltip("ī�޶� ������ ��, �ش� �� ��ŭ �߰��� �� �����Դϴ�. ")]
    [Range(0f, 5f)]
    public float moveValue_Transform;

    [Tooltip("false�� �� ī�޶� �i�ư���, true�� �� �÷��̾ �i�ư��ϴ�.")]
    public bool followPlayer;
    public Transform target;

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


        if (target == null) //null�� ���
        {
            if (followPlayer)
            {
                target = PlayerController.Instance.gameObject.transform;
            }
            else
            {

                target = CameraManager.Instance.currentCamera.transform;
            }

        }

        if (moveTex)
        {
            renderer = GetComponent<Renderer>();
            target = CameraManager.Instance.currentCamera.transform;
            followPlayer = false;
            ProcessMove = ProcessMove_Tex;
        }
        else
        {
            ProcessMove = ProcessMove_Transform;
        }

        moverTransform = gameObject.transform;
        prevTargetX = target.transform.position.x;
        mainTex = "_MainTex";

    }
    private void Update()
    {

        ProcessMove();

    }

    private void ProcessMove_Tex()
    {
        moverTransform.position = new Vector2(target.position.x, moverTransform.position.y);
        if (prevTargetX != target.position.x)
        {
            renderer.material.SetTextureOffset(mainTex, new Vector2(target.position.x * moveValue_Tex, 0f));
        }
    }

    private void ProcessMove_Transform()
    {
        if (prevTargetX != target.position.x)
        {
            moverTransform.position = new Vector2(target.position.x, moverTransform.position.y);
            moverTransform.Translate(new Vector2(target.position.x * moveValue_Transform, 0f));

        }
    }
    private void LateUpdate()
    {
        prevTargetX = target.position.x;
    }
}
