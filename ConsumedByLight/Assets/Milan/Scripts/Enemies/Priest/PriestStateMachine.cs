using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestStateMachine : MonoBehaviour
{
    [HideInInspector] public IPriestBaseState currentState;

    [HideInInspector] public PriestSpawnState spawnState = new();
    [HideInInspector] public PriestChaseState chaseState = new();
    [HideInInspector] public PriestAttackState attackState = new();
    [HideInInspector] public PriestDieState dieState = new();
    [HideInInspector] public PriestInactiveState inactiveState = new();
    [HideInInspector] public PriestHealState healState = new();

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject target;

    [SerializeField] public GameObject holyWater;
    [SerializeField] public float force;

    [SerializeField] public float explosiveRadius;
    [SerializeField] public LayerMask enemyLayer;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");

        currentState = chaseState;
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
