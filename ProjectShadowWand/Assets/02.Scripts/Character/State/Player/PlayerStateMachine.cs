using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStateMachine : StateMachine
{
    PlayerController player;
    public PlayerStateMachine(PlayerController _player)
    {
        player = _player;
        prevStateE = eState.NONE;
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

            case eState.PLAYER_GLIDE:
                tempState = new PlayerState_Glide(player);
                break;
            //case eState.PLAYER_CATCH:
            //    tempState = new PlayerState_Catch(player);
            //    break;
            case eState.PLAYER_SKILL_WATER:
                tempState = new PlayerState_Skill_Water(player);
                break;

            case eState.PLAYER_SKILL_LIGHTNING:
                tempState = new PlayerState_Skill_Lightning(player);
                break;

            case eState.PLAYER_FALL:
                tempState = new PlayerState_Fall(player);
                break;

            case eState.PLAYER_DIE:
                tempState = new PlayerState_Die(player);
                break;

            //case eState.PLAYER_PUSH:
            //    tempState = new PlayerState_Push(player);
            //    break;

            default:
                break;
        }
        return tempState;
    }
}
