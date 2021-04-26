using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStateMachine : StateMachine
{
    PlayerController player;
    public PlayerStateMachine(PlayerController _player)
    {
        player = _player;
    }
    public void Start()
    {
        //Debug.Log("스테이트 머신 기동 확인");
        //Debug.Log("현재 스테이트 : " + currentState);
    }
    public override void ChangeState(eState _state)
    {
        base.ChangeState(_state);
    }
    public override State GetState(eState _state)
    {
        PlayerState tempState = new PlayerState();

        switch (_state)
        {
            case eState.PLAYER_DEFAULT:
                tempState = new PlayerState_Default(player);
                break;

            case eState.PLAYER_JUMP:
                tempState = new PlayerState_Jump(player);
                break;

            case eState.PLAYER_CLIMB_LADDER:
                tempState = new PlayerState_Climb_Ladder(player);
                break;

            case eState.PLAYER_PUSH:
                tempState = new PlayerState_Push(player);
                break;

            case eState.PLAYER_LIFT:
                tempState = new PlayerState_Lift(player);
                break;

            case eState.PLAYER_AIR:
                tempState = new PlayerState_Air(player);
                break;

            case eState.PLAYER_DIE:
                tempState = new PlayerState_Die(player);
                break;

            default:
                break;
        }
        return tempState;
    }
}
