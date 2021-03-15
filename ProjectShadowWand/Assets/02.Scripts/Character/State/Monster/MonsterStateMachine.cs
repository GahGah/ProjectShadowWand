using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{

    Monster monster;
    public MonsterStateMachine(Monster _monster)
    {
        monster = _monster;
    }

    public void Start()
    {
        Debug.Log("몬스터 스테이트 머신 기동 확인");
        Debug.Log("현재 스테이트 : " + currentState);
    }

    public override void ChangeState(eSTATE _state)
    {
        base.ChangeState(_state);
    }

    public override State GetState(eSTATE _state)
    {
        MonsterState tempState = new MonsterState();
        switch (_state)
        {
            case eSTATE.MONSTER_DEFAULT:
                if (monster.monsterType==eMonsterType.A) //A타입 일 경우
                {
                    tempState = new MonsterStateA_Defalut(monster);
                }
                break;

            case eSTATE.MONSTER_CHASE:
                if (monster.monsterType == eMonsterType.A) //A타입 일 경우
                {
                    tempState = new MonsterStateA_Defalut(monster);
                }
                break;

            case eSTATE.MONSTER_OUTSHADOW:
                if (monster.monsterType == eMonsterType.A) //A타입 일 경우
                {
                    tempState = new MonsterStateA_Defalut(monster);
                }
                break;

            case eSTATE.MONSTER_DIE:
                if (monster.monsterType == eMonsterType.A) //A타입 일 경우
                {
                    tempState = new MonsterStateA_Defalut(monster);
                }
                break;

            default:
                break;
        }
        return tempState;
    }
}
