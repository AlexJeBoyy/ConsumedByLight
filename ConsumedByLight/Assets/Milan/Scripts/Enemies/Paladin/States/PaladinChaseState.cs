using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinChaseState : IPaladinBaseState
{
    float refreshtime = 0.5f;
    float timer;
    public void Exit(PaladinStateMachine paladin)
    {
        timer = refreshtime;
    }

    public void Start(PaladinStateMachine paladin)
    {
        timer = refreshtime;
        paladin.agent.SetDestination(paladin.target.transform.position);
    }

    public void Update(PaladinStateMachine paladin)
    {
        UpdateTimer();

        if (timer <= 0)
        {
            timer = refreshtime;
            paladin.agent.SetDestination(paladin.target.transform.position);
        }

        if (paladin.agent.remainingDistance <= paladin.agent.stoppingDistance)
        {
            paladin.SwitchState(paladin.attackState);
        }
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }
}
