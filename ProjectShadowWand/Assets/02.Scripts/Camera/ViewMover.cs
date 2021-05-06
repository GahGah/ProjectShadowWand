using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라의 움직임에 맞춰서 오브젝트의 offset을 변경시킵니다.
/// </summary>
public class ViewMover : MonoBehaviour
{
    [Header("움직이는 정도"), Tooltip("카메라가 움직일 때, 해당 값 만큼 추가로 더 움직입니다. ")]
    public Vector2 moveValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
