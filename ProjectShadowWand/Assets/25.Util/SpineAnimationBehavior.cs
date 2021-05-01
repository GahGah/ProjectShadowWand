// Spine 3.6 이상에서 작동 합니다.  by seraph
// 구버젼의 경우 내부에서 사용하는 인자가 달라서 에러가 발생할수 있습니다. 

using System.Collections;
using UnityEngine;
using Spine.Unity; // 

public class SpineAnimationBehavior : StateMachineBehaviour //스테이트머신 비헤이비어
{

    [Tooltip("평범한 애니메이션 클립입니다.")]
    public AnimationClip _animationClip;


    [Tooltip("애니메이션 클립의 이름입니다. 내부에선 실제 클립이 아닌 이름이 쓰입니다.")]
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
        //어웨이크에서 쓰지 않아도 될 것 같지만 
        AnimationClipToString(_animationClip);
    }


    private void AnimationClipToString(AnimationClip _ac)
    {

        //스파인 함수들은 따로 스파인.애니메이션 클래스를 쓰지 않으면 그냥 스트링으로 실행할 수 있는 것 같다.
        //근데 나는 이거 잘 모르겠으니까 그냥 기존의 애니메이션클립 클래스를 스트링으로 변환하자.

        if (_animationClip != null) //애니메이션 클립이 있을 때 이름을 받아온다.
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
        if (skeletonAnimation == null) //스켈레톤애니메이션이 없다면
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
        trackEntry.TimeScale = timeScale;

        //0~1의 숫자
        normalizedTime = 0f;


    }

    //스테이트 Update시 실행
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //애니메이션 Last : 아마도 마지막으로 애니메이션이 실행된 시간 느낌임.
        //애니메이션 End : 아마도 애니메이션의 끝나는 시간 느낌.
        //즉, 현재수치/최대치(?) 해서 애니메이션의 실행 게이지(?)를 계산하는듯.
        normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        //루프 설정(bool)을 안했을 경우, exitTime때 애니메이션 전환. 
        if (!loop && normalizedTime >= exitTime)
        {
            animator.SetTrigger("transition");
            //이 트리거 때문에 애니메이터에서 트랜지션이 발생하는 듯. 처음 암.
        }
    }
    // 스테이트 종료시 실행
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}