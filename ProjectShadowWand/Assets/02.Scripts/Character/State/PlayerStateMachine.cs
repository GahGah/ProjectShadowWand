using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(State _state)
    {
        currentState = _state; // 생성할때 state를 넣으면 그게 기본으로....
    }

    public override State GetState(eSTATE _state)
    {
        State tempState = new State();

        switch (_state)
        {
            case eSTATE.PLAYER_DEFAULT:
                //tempState = new AnyPlayerState1();
                Debug.Log("Player_DeFault");
                break;

            case eSTATE.PLAYER_JUMP:
                Debug.Log("Player_Jump");
                break;

            case eSTATE.PLAYER_AIR:
                Debug.Log("Player_Air");
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
