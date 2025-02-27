using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAttackState : IPaladinBaseState
{
    public void Exit(PaladinStateMachine paladin)
    {
        paladin.StopAllCoroutines();
    }

    public void Start(PaladinStateMachine paladin)
    {
        paladin.agent.SetDestination(paladin.transform.position);
        paladin.StartCoroutine(Attack(paladin));
    }

    public void Update(PaladinStateMachine paladin)
    {

    }

    IEnumerator Attack(PaladinStateMachine paladin)
    {
        Debug.Log("Attacking...");

        paladin.swordAnimator.Play("Swing");

        yield return new WaitForSeconds(1);
        Debug.Log("Done Attacking...");
        paladin.SwitchState(paladin.chaseState);

    }
}
