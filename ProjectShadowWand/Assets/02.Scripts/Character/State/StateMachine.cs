using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    //public T GetChildClass<T>() where T : StateMachine
    //{
    //	return (T)this;
    //}

    //현재 스테이트
    protected State currentState;

    //스테이트를 담는 딕셔너리
    //계속 new해서 생성하는건 최적화에 좀 안좋을 것 같아서
    protected Dictionary<eSTATE, State> stateDict = new Dictionary<eSTATE, State>();
    public virtual void ChangeState(eSTATE _state) // 상태를 바꿉니다.
    {

        if (!stateDict.ContainsKey(_state)) //만약 딕셔너리에 state가 안들어있다면 
        {
            State tempState;

            tempState = GetState(_state);

            if (tempState != null) //제대로 가져왔을 경우 
            {
                stateDict[_state] = tempState;
            }
        }
        else // 들어있다면
        {
            if (currentState == stateDict[_state]) //만약 현재 상태와 또 똑같은 상태로 바꾸려고 했다면?
            {
                Debug.LogError("같은 상태로 바뀐다면 좀 이상하지 않을까?");
                return;
            }
        }

        //Exit--
        if (currentState != null) // 먼저 끝내고 
        {
            currentState.Exit();
        }

        //Enter---
        if (stateDict.ContainsKey(_state))
        {
            currentState = stateDict[_state];
            currentState.Enter();
        }
        else
        {
            Debug.LogError("Enter 실패");

        }
    }

    //모노비헤이비어가 아니기 때문에 

    /// <summary>
    /// 익스큐트를 합니다.
    /// </summary>
    public void Update()
    {
        if (currentState != null) // 널이 아닐 경우에만
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// 물리 업데이트를 합니다.
    /// </summary>
    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsExecute();
        }
    }
    public string GetStateName()
    {
        return currentState.ToString();
    }

    /// <summary>
    /// 해당하는 eState에 맞는 State를 반환합니다.
    /// </summary>
    /// <param name="state">enum eSTATE</param>
    public virtual State GetState(eSTATE _state)
    {
        State returnState = new State();
        Debug.LogError("StateMachine GetState를 사용해버림");
        return returnState;
    }


    //public virtual void HandleSelect()
    //{
    //	currentState.HandleInput(State.InputType.Select, false);
    //}

}


