using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestStateMachine : MonoBehaviour
{
    [HideInInspector] public IPriestBaseState currentState;

    [HideInInspector] public PriestSpawnState spawnState;
    [HideInInspector] public PriestChaseState chaseState;
    [HideInInspector] public PriestAttackState attackState;
    [HideInInspector] public PriestDieState dieState;
    [HideInInspector] public PriestInactiveState inactiveState;
    private void Start()
    {
        currentState = spawnState;
        currentState.Start(this);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(IPriestBaseState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Start(this);
    }
}
