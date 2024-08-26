/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_StateMachineBehaviour : StateMachineBehaviour
{
    public States nextState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FSM fsm = animator.gameObject.GetComponent<FSM>();
        if (fsm != null)
        {
            fsm.ChangeState(nextState);
        }
    }

}*/
