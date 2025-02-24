using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossStateMachine : MonoBehaviour
{
    [HideInInspector] public ICrossBaseState currentState;

    [HideInInspector] public CrossSpawnState spawnState = new();
    [HideInInspector] public CrossChaseState chaseState = new();
    [HideInInspector] public CrossAttackState attackState = new();
    [HideInInspector] public CrossDieState dieState = new();
    [HideInInspector] public CrossInactiveState inactiveState = new();

    private void Start()
    {
        currentState = spawnState;
        currentState.Start(this);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(ICrossBaseState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Start(this);
    }
}
