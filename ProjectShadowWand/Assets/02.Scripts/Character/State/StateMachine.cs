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

    [Tooltip("현재 상태입니다.")]
    protected eState currentStateE;

    [Tooltip("이전 상태입니다.")]
    protected eState prevStateE;

    //스테이트를 담는 딕셔너리
    //계속 new해서 생성하는건 최적화에 좀 안좋을 것 같아서
    protected Dictionary<eState, State> stateDict = new Dictionary<eState, State>();
    public virtual void ChangeState(eState _state) // 상태를 바꿉니다.
    {

        var goReturn = false;
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
                goReturn = true;
            }
        }

        if (goReturn == true)
        {
            return;
        }
        
        if (currentState != null) // 먼저 끝내고 
        {
            currentState.Exit();
        }

        if (stateDict.ContainsKey(_state))
        {
            prevStateE = currentStateE;
            currentState = stateDict[_state];
            currentStateE = _state;
            currentState.Enter();
            //Debug.Log("현재 상태 : " + GetCurrentStateName());
        }
        else
        {
            Debug.LogError("Enter 실패");

        }
    }

    //모노비헤이비어가 아니니까 자유롭게 사용

    /// <summary>
    /// Update를 합니다.
    /// </summary>
    public void Update()
    {
        if (currentState != null) // 널이 아닐 경우에만
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// FixedUpdate를 합니다.
    /// </summary>
    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsExecute();
        }
    }


    /// <summary>
    /// 현재 state를 eState형식으로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public eState GetCurrentStateE()
    {
        return currentStateE;
    }

    /// <summary>
    /// 이전 상태의 eState형식을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public eState GetPrevStateE()
    {
        return prevStateE;
    }
    public string GetCurrentStateName()
    {
        if (currentState != null)
        {
            return currentState.ToString();
        }
        else
        {
            return string.Empty;
        }

    }

    /// <summary>
    /// 해당하는 eState에 맞는 State를 반환합니다.
    /// </summary>
    /// <param name="state">enum eState</param>
    public virtual State GetState(eState _state)
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


