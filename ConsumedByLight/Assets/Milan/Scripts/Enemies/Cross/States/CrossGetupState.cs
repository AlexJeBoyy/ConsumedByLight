using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossGetupState : ICrossBaseState
{
    public void Exit(CrossStateMachine cross)
    {

    }

    public void Start(CrossStateMachine cross)
    {
        Debug.Log("Entered grabed state");
        cross.StartCoroutine(Getup(cross));
    }

    public void Update(CrossStateMachine cross)
    {

    }

    IEnumerator Getup(CrossStateMachine cross)
    {
        yield return new WaitForSeconds(4f);

        cross.GetComponent<NavMeshAgent>().enabled = true;
        cross.GetComponent<Rigidbody>().isKinematic = true;
        cross.GetComponent<Rigidbody>().useGravity = false;

        cross.SwitchState(cross.chaseState);
    }
}
