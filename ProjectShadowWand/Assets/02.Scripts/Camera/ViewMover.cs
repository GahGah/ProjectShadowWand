using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라의 움직임에 맞춰서 오브젝트의 offset을 변경시킵니다.
/// </summary>
public class ViewMover : MonoBehaviour
{

    delegate void ProcessMoveDelegate();
    ProcessMoveDelegate ProcessMove;
    public bool moveTex;

    [Header("moveTex 체크"), Tooltip("카메라가 움직일 때, 해당 값 만큼 추가로 더 움직입니다. ")]
    [Range(0f, 0.05f)]
    public float moveValue_Tex;

    [Header("moveTex 체크 X"), Tooltip("카메라가 움직일 때, 해당 값 만큼 추가로 더 움직입니다. ")]
    [Range(0f, 5f)]
    public float moveValue_Transform;

    [Tooltip("false일 때 카메라를 쫒아가고, true일 때 플레이어를 쫒아갑니다.")]
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
    //        if (object.ReferenceEquals(_moverTransform, null)) //최적화를 위해 레퍼런스이퀄 사용
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


        if (target == null) //null일 경우
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
