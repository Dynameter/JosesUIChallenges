using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A state machine that can take custom states.
/// </summary>
public class StateMachine
{
    /// <summary>
    /// The current state that is running.
    /// </summary>
    private State _currentState;

    /// <summary>
    /// Attempts to set the state to the given state type.
    /// </summary>
    /// <typeparam name="StateType">The state we want to switch to</typeparam>
    /// <returns>Returns true if we were able to switch to the given state.</returns>
    public bool SetCurrentStateTo<StateType>() where StateType : State, new()
    {
        return SetCurrentStateTo(new StateType());
    }

    /// <summary>
    /// Attempts to set the state to the given state.
    /// </summary>
    /// <param name="argStateToSwitchTo">The switch to state to.</param>
    /// <returns>Returns true if we were able to switch to the given state.</returns>
    public bool SetCurrentStateTo(State argStateToSwitchTo)
    {
        if (this._currentState != null)
        {
            //First, check if we can switch states. If we can, exit the previous state
            if (_currentState.CanSwitchState(argStateToSwitchTo) == true)
            {
                this._currentState.Exit();
            }
            else
            {
                return false;
            }
        }

        //Switch into the passed in state
        this._currentState = argStateToSwitchTo;
        this._currentState.Enter();

        return true;
    }

    /// <summary>
    /// Gets the current state the state machine is running.
    /// </summary>
    /// <returns></returns>
    public State GetCurrentState()
    {
        return this._currentState;
    }

    /// <summary>
    /// Updates the running state.
    /// </summary>
    public void UpdateState()
    {
        if (this._currentState != null)
        {
            this._currentState.Update();
        }
    }

    /// <summary>
    /// Checks to see if the running state is of the given type.
    /// </summary>
    /// <typeparam name="StateType">The state type to check against.</typeparam>
    /// <returns>Returns true if the running state if of the same type.</returns>
    public bool IsCurrentStateOfType<StateType>() where StateType : State
    {
        return (this._currentState is StateType);
    }
}