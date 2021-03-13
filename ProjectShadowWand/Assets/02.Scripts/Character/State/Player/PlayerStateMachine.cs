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
        Debug.Log("스테이트 머신 기동 확인");
        Debug.Log("현재 스테이트 : " + currentState);
    }
    public override void ChangeState(eSTATE _state)
    {
        base.ChangeState(_state);
    }
    public override State GetState(eSTATE _state)
    {
        PlayerState tempState = new PlayerState();

        switch (_state)
        {
            case eSTATE.PLAYER_DEFAULT:
                tempState = new PlayerState_Default(player);
                break;

            case eSTATE.PLAYER_JUMP:
                tempState = new PlayerState_Jump(player);
                break;
            case eSTATE.PLAYER_AIR:
                tempState = new PlayerState_Air(player);
                break;

            case eSTATE.PLAYER_DIE:
                Debug.Log("Player_Die");
                break;
            default:
                break;
        }
        return tempState;
    }
}
