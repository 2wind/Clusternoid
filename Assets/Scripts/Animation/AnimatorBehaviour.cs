using System;
using UnityEngine;

public class AnimatorBehaviour : StateMachineBehaviour
{
    public enum ActionType
    {
        MoveForward,
        MoveSideways,
        MoveRandom,
        Accelerate,
        ChooseRotation,
        Fire,
        Rotate,
        TowardRandom,
        Death,
        ChooseDirection,
        LookAt,
        PathFind,
        FindNearestCharacter,
        SetSuperArmor
    }

    [Serializable]
    public struct Action
    {
        public ActionType type;
        public string key;
        public float value;
    }

    public Action[] onEnter;
    public Action[] update;
    public Action[] onExit;

    [NonSerialized] public AnimationAction action;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var act in onEnter)
        {
            action.PlayAction(act);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var act in update)
        {
            action.PlayAction(act);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var act in onExit)
        {
            action.PlayAction(act);
        }
    }


    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
    //
    //}

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    //
    //}
}