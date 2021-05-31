using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAnimationPlay : MonoBehaviour
{
    [HideInInspector]
    public int animatorCollisionEnterTrigger;
    public Animator animator;
    public Animation myAnimation;

    private List<string> animationList;
    int index = 0;

    private void Awake()
    {
        animatorCollisionEnterTrigger = Animator.StringToHash("CollisionEnter");
        //Init_Animation();
        //myAnimation.wrapMode = WrapMode.Once;
    }


    private void Init_Animation()
    {
        animationList = new List<string>();
        foreach (AnimationState state in myAnimation)
        {
            animationList.Add(state.name);
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger(animatorCollisionEnterTrigger);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetTrigger(animatorCollisionEnterTrigger);
    }


}
