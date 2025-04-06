using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PaladinGetupState : IPaladinBaseState
{
    public void Exit(PaladinStateMachine paladin)
    {

    }

    public void Start(PaladinStateMachine paladin)
    {
        Debug.Log("Entered grabed state");
        paladin.StartCoroutine(Getup(paladin));
    }

    public void Update(PaladinStateMachine paladin)
    {

    }

    IEnumerator Getup(PaladinStateMachine paladin)
    {
        yield return new WaitForSeconds(4f);

        paladin.GetComponent<NavMeshAgent>().enabled = true;
        paladin.GetComponent<Rigidbody>().isKinematic = true;
        paladin.GetComponent<Rigidbody>().useGravity = false;

        paladin.SwitchState(paladin.chaseState);
    }
}
