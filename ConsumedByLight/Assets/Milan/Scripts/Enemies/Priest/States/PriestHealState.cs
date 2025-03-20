using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestHealState : IPriestBaseState
{
    public void Exit(PriestStateMachine priest)
    {
        
    }

    private void CheckHit(PriestStateMachine priest)
    {
        Collider[] hitObjects = Physics.OverlapSphere(priest.transform.position, priest.explosiveRadius, priest.enemyLayer);

        if (hitObjects.Length > 1)
        {
            int randomindex = Random.Range(1, hitObjects.Length);

            priest.transform.LookAt(hitObjects[randomindex + 0].gameObject.transform.position);
        }
        else
        {
            priest.SwitchState(priest.chaseState);
        }
    }

    public void Start(PriestStateMachine priest)
    {
        CheckHit(priest);
        priest.StartCoroutine(Attack(priest));
    }

    public void Update(PriestStateMachine priest)
    {
        
    }

    IEnumerator Attack(PriestStateMachine priest)
    {
        Shoot(priest);
        yield return new WaitForSeconds(2);
        priest.SwitchState(priest.chaseState);

    }

    void Shoot(PriestStateMachine priest)
    {
        GameObject water = Object.Instantiate(priest.holyWater, priest.transform.position, Quaternion.identity);
        water.GetComponent<Rigidbody>().AddForce(priest.transform.forward.normalized * priest.force, ForceMode.Impulse);
        water.GetComponent<Rigidbody>().AddForce(priest.transform.up.normalized * priest.force, ForceMode.Impulse);
    }
}
