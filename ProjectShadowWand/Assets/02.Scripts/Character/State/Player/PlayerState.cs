using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public PlayerController player;
    public override void Enter() { }
    public override void Execute()
    { }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute() { }
    public override void Exit() { base.Exit(); }

    public override void HandleInput() { }
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
        player.animator.SetFloat(player.animatorRunningSpeed, Mathf.Abs(player.saveMoveInputX));
    }
    public override void Execute()
    {
    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {
        player.animator.SetFloat(player.animatorRunningSpeed, Mathf.Abs(player.saveMoveInputX));
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
        isJumped = false; ;

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
            Log("Enter Jump");
            player.animator.SetTrigger(player.animatorJumpTrigger);

            //점프
            player.playerRigidbody.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);
            //점프 트리거 온


            //점프 입력을 false로(점프를 한번만 하기 위해서)
            player.jumpInput = false;
            //점프상태 true
            player.isJumping = true;
            isJumped = true;
            // Play audio
            //audioPlayer.PlayJump();

        }
    }
    public override void Exit()
    {
        Log("Exit Jump");
    }

    public override void HandleInput() { }
}

public class PlayerState_Air : PlayerState
{
    public PlayerState_Air(PlayerController _p)
    {
        player = _p;
    }
    public override void Enter()
    {
        Debug.Log("Enter Air");
    }
    public override void Execute()
    {

    }

    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute() { }
    public override void Exit()
    {
        Log("Exit Air");

        // Play audio
        //audioPlayer.PlayLanding(blockType);
    }

    public override void HandleInput() { }
}