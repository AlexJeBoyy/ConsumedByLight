using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrossBaseState
{
    public void Start(CrossStateMachine cross);
    public void Update(CrossStateMachine cross);
    public void Exit(CrossStateMachine cross);
}
