using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPaladinBaseState
{
    public void Start(PaladinStateMachine paladin);

    public void Update(PaladinStateMachine paladin);

    public void Exit(PaladinStateMachine paladin);

}
