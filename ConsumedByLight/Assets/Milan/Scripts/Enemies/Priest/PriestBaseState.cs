using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPriestBaseState
{
    public void Start(PriestStateMachine priest);

    public void Update(PriestStateMachine priest);

    public void Exit(PriestStateMachine priest);
}
