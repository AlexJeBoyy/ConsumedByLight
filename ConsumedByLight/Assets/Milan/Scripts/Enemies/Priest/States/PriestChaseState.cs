using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestChaseState : IPriestBaseState
{
    float refreshtime = 0.5f;
    float timer;
    public void Exit(PriestStateMachine priest)
    {
        timer = refreshtime;
    }

    public void Start(PriestStateMachine priest)
    {
        timer = refreshtime;
        priest.agent.SetDestination(priest.target.transform.position);
    }

    public void Update(PriestStateMachine priest)
    {
        UpdateTimer();

        if (timer <= 0)
        {
            timer = refreshtime;
            priest.agent.SetDestination(priest.target.transform.position);
        }

        if (priest.agent.remainingDistance <= priest.agent.stoppingDistance)
        {
            priest.SwitchState(priest.attackState);
        }
    }
    void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }
}
