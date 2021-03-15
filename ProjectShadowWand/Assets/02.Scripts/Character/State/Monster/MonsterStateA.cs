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
public class MonsterStateA_Chase : MonsterState
{
    public MonsterStateA_Chase(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {

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
public class MonsterStateA_OutShadow : MonsterState
{
    public MonsterStateA_OutShadow(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {

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


public class MonsterStateA_Die : MonsterState
{
    public MonsterStateA_Die(Monster _m)
    {
        monster = _m;
    }
    public override void Enter()
    {

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