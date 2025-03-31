using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossStateMachine : MonoBehaviour
{
    [HideInInspector] public ICrossBaseState currentState;

    [HideInInspector] public CrossSpawnState spawnState = new();
    [HideInInspector] public CrossChaseState chaseState = new();
    [HideInInspector] public CrossAttackState attackState = new();
    [HideInInspector] public CrossDieState dieState = new();
    [HideInInspector] public CrossInactiveState inactiveState = new();

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject target;

    [SerializeField] public Animator crossAnimator;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip smashClip;
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

    public void SwitchState(ICrossBaseState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Start(this);
    }
}
