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

}
