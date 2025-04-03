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
        
        paladin.swordAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        paladin.isAttacking = true;
        yield return new WaitForSeconds(0.4f);
        paladin.isAttacking = false;
        yield return new WaitForSeconds(0.2f);
        paladin.SwitchState(paladin.chaseState);

    }
}
