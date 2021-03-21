using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("캐릭터 관련")]
    [HideInInspector] public Animator animator = null;
    [HideInInspector] public Transform puppet = null;
    [HideInInspector] public SpriteRenderer spriteRenderer= null;

    [Header("이동 관련")]
    public Vector2 movementInput;

    [Tooltip("이동속도")] 
    public float movementSpeed = 0.0f;
    [Tooltip("플레이어의 최대 이동속도. \n가속도를 더하는 형식이라 필요한 것 뿐입니다.")]
    public float maxMovementSpeed = 0.0f;
    public float jumpForce = 0.0f;

    [HideInInspector] public float minFlipSpeed = 0.1f;
    [HideInInspector] public readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    [HideInInspector] public bool isFlipped;

}
