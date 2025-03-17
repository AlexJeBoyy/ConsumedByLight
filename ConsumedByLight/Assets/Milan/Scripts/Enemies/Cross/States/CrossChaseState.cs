using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossChaseState : ICrossBaseState
{
    float refreshtime = 0.5f;
    float timer;
    public void Exit(CrossStateMachine cross)
    {
        timer = refreshtime;
    }

    public void Start(CrossStateMachine cross)
    {
        timer = refreshtime;
        cross.agent.SetDestination(cross.target.transform.position);
    }

    public void Update(CrossStateMachine cross)
    {
        UpdateTimer();

        if (timer <= 0)
        {
            timer = refreshtime;
            cross.agent.SetDestination(cross.target.transform.position);
        }

        if (cross.agent.remainingDistance <= cross.agent.stoppingDistance)
        {
            cross.SwitchState(cross.attackState);
        }
    }
    void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }
}
