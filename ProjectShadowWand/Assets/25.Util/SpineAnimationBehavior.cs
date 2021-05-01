// Spine 3.6 �̻󿡼� �۵� �մϴ�.  by seraph
// �������� ��� ���ο��� ����ϴ� ���ڰ� �޶� ������ �߻��Ҽ� �ֽ��ϴ�. 

using System.Collections;
using UnityEngine;
using Spine.Unity; // 

public class SpineAnimationBehavior : StateMachineBehaviour //������Ʈ�ӽ� �����̺��
{

    [Tooltip("����� �ִϸ��̼� Ŭ���Դϴ�.")]
    public AnimationClip _animationClip;


    [Tooltip("�ִϸ��̼� Ŭ���� �̸��Դϴ�. ���ο��� ���� Ŭ���� �ƴ� �̸��� ���Դϴ�.")]
    private string animationClip;

    //layer to play animation on
    public int layer = 0;

    //for playing the anim at a different timescale if desired
    public float timeScale = 1f;

    private float normalizedTime;
    public float exitTime = .85f;
    public bool loop;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimationState;
    private Spine.TrackEntry trackEntry;

    private void Awake()
    {
        //�����ũ���� ���� �ʾƵ� �� �� ������ 
        AnimationClipToString(_animationClip);
    }


    private void AnimationClipToString(AnimationClip _ac)
    {

        //������ �Լ����� ���� ������.�ִϸ��̼� Ŭ������ ���� ������ �׳� ��Ʈ������ ������ �� �ִ� �� ����.
        //�ٵ� ���� �̰� �� �𸣰����ϱ� �׳� ������ �ִϸ��̼�Ŭ�� Ŭ������ ��Ʈ������ ��ȯ����.

        if (_animationClip != null) //�ִϸ��̼� Ŭ���� ���� �� �̸��� �޾ƿ´�.
        {
            animationClip = _animationClip.name;
        }
        else
        {

        }
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animationClip == null)
        {
   
        }
        if (skeletonAnimation == null) //���̷���ִϸ��̼��� ���ٸ�
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
        trackEntry.TimeScale = timeScale;

        //0~1�� ����
        normalizedTime = 0f;


    }

    //������Ʈ Update�� ����
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�ִϸ��̼� Last : �Ƹ��� ���������� �ִϸ��̼��� ����� �ð� ������.
        //�ִϸ��̼� End : �Ƹ��� �ִϸ��̼��� ������ �ð� ����.
        //��, �����ġ/�ִ�ġ(?) �ؼ� �ִϸ��̼��� ���� ������(?)�� ����ϴµ�.
        normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        //���� ����(bool)�� ������ ���, exitTime�� �ִϸ��̼� ��ȯ. 
        if (!loop && normalizedTime >= exitTime)
        {
            animator.SetTrigger("transition");
            //�� Ʈ���� ������ �ִϸ����Ϳ��� Ʈ�������� �߻��ϴ� ��. ó�� ��.
        }
    }
    // ������Ʈ ����� ����
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}