using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public PlayerController player;
    protected void ResetAnimatorSpeed()
    {
        player.animator.speed = 1f;
    }
}

public class PlayerState_Default : PlayerState
{
    public PlayerState_Default(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        eState tempState = player.playerStateMachine.GetPrevStateE();
        if (tempState == eState.PLAYER_GLIDE)
        {
            player.flipFX.SetActive(true);
        }

        Log("Enter Default");
        ResetAnimatorSpeed();
        if ((InputManager.Instance.buttonMoveRight.isPressed || InputManager.Instance.buttonMoveLeft.isPressed) && Mathf.Abs(player.playerRigidbody.velocity.x) > 0f)
        {
            player.animator.SetBool(player.animatorWalkingBool, true);
        }
        else
        {
            player.animator.SetBool(player.animatorWalkingBool, false);
        }
    }
    public override void Execute()
    {

    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {
        if ((InputManager.Instance.buttonMoveRight.isPressed || InputManager.Instance.buttonMoveLeft.isPressed) && Mathf.Abs(player.playerRigidbody.velocity.x) > 0f)
        {
            player.animator.SetBool(player.animatorWalkingBool, true);
        }
        else
        {
            player.animator.SetBool(player.animatorWalkingBool, false);
        }

    }
    public override void Exit()
    {
        Log("Exit Default");
        player.animator.SetBool(player.animatorWalkingBool, false);
    }

    public override void HandleInput() { }
}

public class PlayerState_Jump : PlayerState
{
    private bool isJumped;
    public PlayerState_Jump(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        Log("Enter Jump");
        ResetAnimatorSpeed();
        isJumped = false;
        player.animator.SetBool(player.animatorJumpingBool, true);
        player.animator.SetTrigger(player.animatorJumpTrigger);
        player.animator.SetBool(player.animatorGroundedBool, false);

    }
    public override void Execute()
    {

    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {
        if (!isJumped)
        {
            Log("PE Jump");
            isJumped = true;
            player.animator.SetBool(player.animatorJumpingBool, true);
            //점프 트리거 온
            player.animator.SetTrigger(player.animatorJumpTrigger);

            //점프
            //player.playerRigidbody.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);


            ////점프 입력을 false로(점프를 한번만 하기 위해서)
            //player.jumpInput = false;
            ////점프상태 true
            //player.isJumping = true;

            // Play audio
            //audioPlayer.PlayJump();

        }

    }
    public override void Exit()
    {
        player.animator.SetBool(player.animatorJumpingBool, false);

    }

    public override void HandleInput()
    {

    }
}

public class PlayerState_Glide : PlayerState // = wind
{
    public PlayerState_Glide(PlayerController _p)
    {
        player = _p;
    }

    public override void Enter()
    {

        player.animator.SetBool(player.animatorFallingBool, false);
        player.animator.SetBool(player.animatorGlidingBool, true);
        player.ResetWindStart();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        player.animator.SetBool(player.animatorGlidingBool, false);
        if (player.GlideCoroutine != null)
        {
            player.StopCoroutine(player.GlideCoroutine);

        }

        player.ResetWindEnd();
    }

    public override void HandleInput()
    {
    }

    public override void PhysicsExecute()
    {
    }
}
public class PlayerState_Fall : PlayerState
{
    public PlayerState_Fall(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        player.animator.SetBool(player.animatorFallingBool, true);
    }
    public override void Execute()
    {

    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {


    }
    public override void Exit()
    {
        player.animator.SetBool(player.animatorFallingBool, false);

        eState tempState = player.playerStateMachine.GetPrevStateE();
        if (tempState == eState.PLAYER_JUMP)
        {
            player.landedFX.SetActive(true);
        }
        else if (tempState == eState.PLAYER_GLIDE)
        {
            player.flipFX.SetActive(true);
        }
        // Play audio
        //audioPlayer.PlayLanding(blockType);
    }

    public override void HandleInput() { }
}

public class PlayerState_Climb_Ladder : PlayerState
{
    private float animatorSpeed = 0f;
    public PlayerState_Climb_Ladder(PlayerController _p)
    {
        player = _p;
        animatorSpeed = 1.5f;
    }

    public override void Enter()
    {
        Log("Enter Ladder");
        ResetAnimatorSpeed();
        player.animator.SetBool(player.animatorClimbingBool, true);
        player.animator.SetBool(player.animatorWalkingBool, false);

    }
    public override void Execute()
    {
        player.animator.SetBool(player.animatorClimbingBool, true);
        //이전 위치와 현재 위치가 다를 경우 
        if (player.prevPosition != player.playerRigidbody.position)
        {
            if (player.animator.speed != animatorSpeed)
            {
                player.animator.speed = animatorSpeed;
            }
        }
        else
        {
            if (player.animator.speed != 0f)
            {
                player.animator.speed = 0f;
            }

        }

    }
    public override void PhysicsExecute()
    {


    }
    public override void Exit()
    {
        Log("Exit Ladder");
        ResetAnimatorSpeed();
        player.animator.SetBool(player.animatorClimbingBool, false);
    }

    public override void HandleInput() { }

}

public class PlayerState_Skill_Lightning : PlayerState
{
    public PlayerState_Skill_Lightning(PlayerController _p)
    {
        player = _p;
    }

    public override void Enter()
    {
        player.animator.SetBool(player.animatorLightningBool, true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        player.animator.SetBool(player.animatorLightningBool, false);
    }

    public override void PhysicsExecute()
    {
    }
}

public class PlayerState_Skill_Water : PlayerState
{
    public PlayerState_Skill_Water(PlayerController _p)
    {
        player = _p;
    }

    public override void Enter()
    {
        player.animator.SetBool(player.animatorWaterBool, true);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        player.animator.SetBool(player.animatorWaterBool, false);

    }

    public override void PhysicsExecute()
    {
    }
}

public class PlayerState_Interact : PlayerState
{

    public PlayerState_Interact(PlayerController _p)
    {
        player = _p;
    }

    public override void Enter()
    {
        player.animator.SetBool(player.animatorInteractingBool, true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        player.animator.SetBool(player.animatorInteractingBool, false);
    }

    public override void PhysicsExecute()
    {
    }
}
//public class PlayerState_Skill_Wind : PlayerState
//{

//}
public class PlayerState_Die : PlayerState
{
    public PlayerState_Die(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        ResetAnimatorSpeed();
        player.animator.SetBool(player.animatorDieBool, true);
        player.sceneChanger.LoadScene_Die();
    }
    public override void Execute()
    {

    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {

    }
    public override void Exit()
    {
        ResetAnimatorSpeed();
    }

    public override void HandleInput()
    {

    }
}

//public class PlayerState_Push : PlayerState
//{
//    private float animatorSpeed;
//    public PlayerState_Push(PlayerController _p)
//    {
//        player = _p;
//        animatorSpeed = 1f;
//    }
//    public override void Enter()
//    {
//        ResetAnimatorSpeed();
//        player.animator.SetBool(player.animatorPushingBool, true);
//    }
//    public override void Execute()
//    {
//        player.animator.SetBool(player.animatorPushingBool, true);

//        //if ((InputManager.Instance.buttonMoveRight.isPressed && Mathf.Abs(player.playerRigidbody.velocity.x) > 0f))
//        //{

//        //    //player.animator.SetBool(player.animatorWalkingBool, true);
//        //}
//        //else
//        //{
//        //    player.animator.SetBool(player.animatorWalkingBool, false);
//        //}

//        //이전 위치와 현재 위치가 다를 경우 
//        if (player.prevPosition != player.playerRigidbody.position)
//        {
//            if (player.animator.speed != animatorSpeed)
//            {
//                player.animator.speed = animatorSpeed;

//            }

//        }
//        else
//        {
//            if (player.animator.speed != 0f)
//            {
//                player.animator.speed = 0f;
//            }

//        }





//    }

//    public override void PhysicsExecute()
//    {


//    }
//    public override void Exit()
//    {
//        Log("Exit Ladder");
//        ResetAnimatorSpeed();
//        player.animator.SetBool(player.animatorPushingBool, false);
//        // player.animator.SetBool(player.animatorClimbBool, false);
//    }

//    //플레이어에 들어갔던 푸시 관련 함수들...
//    //public void CheckPushInput(PushableObject _obj)
//    //{
//    //    if (CanMove())
//    //    {
//    //        var tempObj = _obj;

//    //        if (InputManager.Instance.buttonPush.wasPressedThisFrame)// 키 누르기
//    //        {
//    //            if (pushedObject == null) //밀어야 할 경우
//    //            {
//    //                if (touchedObject != null)
//    //                {
//    //                    SetPushedObject(tempObj);

//    //                    if (pushedObject != null)
//    //                    {
//    //                        pushedObject.GoPushReady();
//    //                    }
//    //                }
//    //            }
//    //        }

//    //        if (InputManager.Instance.buttonPush.wasReleasedThisFrame) // 키 떼기
//    //        {
//    //            if (pushedObject != null)
//    //            {
//    //                pushedObject.GoPutThis();
//    //                pushedObject = null;
//    //            }
//    //        }
//    //        if (InputManager.Instance.buttonPush.isPressed) //키 계속 누르기 
//    //        {
//    //            if (pushedObject == null) //밀어야 할 경우
//    //            {
//    //                if (touchedObject != null)
//    //                {
//    //                    SetPushedObject(tempObj);

//    //                    if (pushedObject != null)
//    //                    {
//    //                        pushedObject.GoPushReady();

//    //                    }
//    //                }
//    //            }
//    //            else
//    //            {
//    //                pushedObject.GoPushThis();
//    //            }
//    //        }

//    //    }


//    //}



//    //public void SetPushedObject(PushableObject _po)
//    //{
//    //    pushedObject = _po;
//    //}
//    //public PushableObject GetPushedObject()
//    //{
//    //    return pushedObject;
//    //}


//if (pushedObject != null)//뭔가 밀고 있는 오브젝트가 있을 때
//{

//    isPushing = true;
//    var tempVelo = pushedObject.rigidBody.velocity;
//    var testSpeed = movementSpeed;
//    if (isRight)
//    {
//        if (movementInput == Vector2.left)
//        {
//            pushedObject.rigidBody.velocity = Vector2.zero;
//            tempVelo += Vector2.left;

//            var vec = new Vector2(Mathf.Clamp(tempVelo.x, -testSpeed, testSpeed), Mathf.Clamp(tempVelo.y, -testSpeed, testSpeed));
//            pushedObject.rigidBody.velocity += vec;
//            //pushedObject.rigidBody.velocity = playerRigidbody.velocity;
//        }
//    }
//    else
//    {
//        if (movementInput == Vector2.right)
//        {
//            //  pushedObject.rigidBody.velocity += (Vector2.right);;
//            pushedObject.rigidBody.velocity = Vector2.zero;
//            tempVelo += Vector2.right;

//            var vec = new Vector2(Mathf.Clamp(tempVelo.x, -testSpeed, testSpeed), Mathf.Clamp(tempVelo.y, -testSpeed, testSpeed));
//            pushedObject.rigidBody.velocity += vec;
//            //pushedObject.rigidBody.velocity = playerRigidbody.velocity;
//        }
//    }

//}
//else
//{
//    isPushing = false;
//}


//}
