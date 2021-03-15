using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateA_Defalut : MonsterState
{
    public MonsterStateA_Defalut(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {
        monster.animator.SetBool(monster.animatorIdleBool, true);
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
        monster.animator.SetBool(monster.animatorIdleBool, false);
    }

}
public class MonsterStateA_Chase : MonsterState
{
    public MonsterStateA_Chase(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {
        monster.animator.SetBool(monster.animatorWalkingBool, true);
    }
    public override void Execute()
    {
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;


        if (monster.targetDistance < 1f) //지정거리 안이면
        {
            Debug.Log("ATTACK!!!");

            moveHorizontal = 0.0f;

        }
        else
        {
            moveHorizontal = monster.targetDir;
        }
        monster.movementInput = new Vector2(moveHorizontal, moveVertical);
    }
    /// <summary>
    /// 혹시 물리용 업데이트를 할까봐...
    /// </summary>
    public override void PhysicsExecute()
    {


    }
    public override void Exit()
    {

        monster.animator.SetBool(monster.animatorWalkingBool, false);
    }

}
public class MonsterStateA_OutShadow : MonsterState
{
    public MonsterStateA_OutShadow(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {

        monster.animator.SetBool(monster.animatorOutShadowBool, true);
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
        monster.animator.SetBool(monster.animatorOutShadowBool, false);
    }

}


public class MonsterStateA_Die : MonsterState
{
    public MonsterStateA_Die(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {
        monster.StartDie();
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
    }

}