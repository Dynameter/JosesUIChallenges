using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A state for the state machine.
/// </summary>
public abstract class State
{
    /// <summary>
    /// Called when the state machine switches to this state.
    /// </summary>
    /// <param name="argArguments">The entry arguments for the state.</param>
    public virtual void Enter(Dictionary<string, object> argArguments) { }

    /// <summary>
    /// Called every frame by the state machine. Updates the state.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Called when the state machine switches to another state.
    /// </summary>
    public virtual void Exit()
    {
        this.CleanUp();
    }

    /// <summary>
    /// Checks if we can transition from this state to the other state.
    /// </summary>
    /// <param name="argStateToSwitchTo">The other state we are requesting to switch to.</param>
    /// <returns>Returns true if we can switch to the given state.</returns>
    public virtual bool CanSwitchState(State argStateToSwitchTo)
    {
        return true;
    }

    /// <summary>
    /// Cleans the state.
    /// </summary>
    public virtual void CleanUp() { }
}