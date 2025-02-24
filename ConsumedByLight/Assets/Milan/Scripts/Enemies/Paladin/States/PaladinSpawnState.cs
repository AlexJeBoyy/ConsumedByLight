using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSpawnState : IPaladinBaseState
{
    public void Exit(PaladinStateMachine paladin)
    {

    }

    public void Start(PaladinStateMachine paladin)
    {
        paladin.SwitchState(paladin.chaseState);
    }

    public void Update(PaladinStateMachine paladin)
    {

    }
}
