using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossAttackState : ICrossBaseState
{
    public void Exit(CrossStateMachine cross)
    {

    }

    public void Start(CrossStateMachine cross)
    {
        cross.StartCoroutine(Attack(cross));
    }

    public void Update(CrossStateMachine cross)
    {

    }

    IEnumerator Attack(CrossStateMachine cross)
    {
        cross.crossAnimator.Play("Swing");

        yield return new WaitForSeconds(2);
        cross.SwitchState(cross.chaseState);

    }
}
