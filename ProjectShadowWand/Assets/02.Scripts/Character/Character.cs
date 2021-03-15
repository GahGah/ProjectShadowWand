using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
    NONE,
    GROUND,
    MOVING_GROUND,
    WALL

}
public class Character : MonoBehaviour
{
    [Header("캐릭터 관련")]
    public Animator animator = null;
    public Transform puppet = null;
    public SpriteRenderer spriteRenderer= null;

    [Header("이동 관련")]

    [Tooltip("이동속도")] 
    public float movementSpeed = 0.0f;

    public float jumpForce = 0.0f;

}
