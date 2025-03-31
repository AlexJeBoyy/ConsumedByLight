using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossChaseState : ICrossBaseState
{
    CrossStateMachine CSM { get; set; }
    float refreshtime = 0.5f;
    float timer;
    public void Exit(CrossStateMachine cross)
    {
        timer = refreshtime;
    }

    public void Start(CrossStateMachine cross)
    {
        if (!cross.agent.enabled) { return; }
        timer = refreshtime;
        cross.agent.SetDestination(cross.target.transform.position);
    }

    public void Update(CrossStateMachine cross)
    {
        if (!cross.agent.enabled) { return; }
        
        UpdateTimer();
        //CSM.audioSource.clip = CSM.draggingClip;
       // CSM.audioSource.loop = true;
        if (timer <= 0)
        {
            timer = refreshtime;
            cross.agent.SetDestination(cross.target.transform.position);
        }

        if (cross.agent.remainingDistance <= cross.agent.stoppingDistance)
        {
            //CSM.audioSource.loop = false;
            cross.SwitchState(cross.attackState);
        }
    }
    void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }
}
