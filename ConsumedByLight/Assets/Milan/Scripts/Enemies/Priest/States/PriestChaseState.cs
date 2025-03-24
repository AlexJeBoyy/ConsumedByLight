using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestChaseState : IPriestBaseState
{
    private enum EnemyState { Attack, Heal }
    private EnemyState curState;
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
            if (priest.agent != null)
            {
                priest.agent.SetDestination(priest.target.transform.position);
            }
        }

        if (priest.agent.remainingDistance <= priest.agent.stoppingDistance)
        {
            DecideNextAction(priest);
        }
    }

    private void DecideNextAction(PriestStateMachine priest)
    {
        curState = Random.Range(0, 2) == 0 ? EnemyState.Attack : EnemyState.Heal;
        if (curState == EnemyState.Attack)
        {
            priest.SwitchState(priest.attackState);
        }
        else
        {
            priest.SwitchState(priest.healState);
        }
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }
}
