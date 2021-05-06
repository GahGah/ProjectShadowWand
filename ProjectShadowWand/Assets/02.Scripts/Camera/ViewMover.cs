using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라의 움직임에 맞춰서 오브젝트의 offset을 변경시킵니다.
/// </summary>
public class ViewMover : MonoBehaviour
{
    public Transform target;
    [Header("움직이는 정도"), Tooltip("카메라가 움직일 때, 해당 값 만큼 추가로 더 움직입니다. ")]
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
            Debug.Log("무버 업데이트");
        }

    }
    private void LateUpdate()
    {
        prevTargetX = target.position.x;
    }

}
