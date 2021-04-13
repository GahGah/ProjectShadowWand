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
        Log("Exit Jump");
    }

    public override void HandleInput()
    {

    }
}

public class PlayerState_Air : PlayerState
{
    public PlayerState_Air(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        Log("Enter Air");
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
        Log("Exit Air");
        // Play audio
        //audioPlayer.PlayLanding(blockType);
    }

    public override void HandleInput() { }
}


public class PlayerState_Climb_Ladder : PlayerState
{
    public PlayerState_Climb_Ladder(PlayerController _p)
    {
        player = _p;
    }

    public override void Enter()
    {
        Log("Enter Ladder");
        ResetAnimatorSpeed();
        player.animator.SetBool(player.animatorClimbBool, true);
    }
    public override void Execute()
    {


        //이전 위치와 현재 위치가 다를 경우 
        if (player.prevPosition != player.playerRigidbody.position)
        {
            player.animator.speed = 1f;
        }
        else
        {
            player.animator.speed = 0f;
        }

    }
    public override void PhysicsExecute()
    {


    }
    public override void Exit()
    {
        Log("Exit Ladder");
        ResetAnimatorSpeed();
        player.animator.SetBool(player.animatorClimbBool, false);
    }

    public override void HandleInput() { }

}