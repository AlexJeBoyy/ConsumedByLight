using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PaladinStateMachine : MonoBehaviour
{
    [HideInInspector] public IPaladinBaseState currentState;

    [HideInInspector] public PaladinSpawnState spawnState = new();
    [HideInInspector] public PaladinChaseState chaseState = new();
    [HideInInspector] public PaladinAttackState attackState = new();
    [HideInInspector] public PaladinDieState dieState = new();
    [HideInInspector] public PaladinInactiveState inactiveState = new();

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject target;
    [HideInInspector] public bool isAttacking = false;

    [SerializeField] public Animator swordAnimator;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");

        currentState = spawnState;
        currentState.Start(this);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(IPaladinBaseState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Start(this);
    }
}
