using UnityEngine;

/// <summary>
/// A monobehaviour wrapper for the state machine.
/// </summary>
public sealed class WordTileStateMachine : MonoBehaviour
{
    /// <summary>
    /// The state machine instance.
    /// </summary>
    private StateMachine _stateMachine = new StateMachine();

    /// <summary>
    /// The state machine instance.
    /// </summary>
    public StateMachine SM
    {
        get
        {
            return this._stateMachine;
        }
    }

    /// <summary>
    /// Sets the starting state
    /// </summary>
    public void Start()
    {

    }

    /// <summary>
    /// Calls update on the state machine.
    /// </summary>
    public void Update()
    {
        _stateMachine.UpdateState();
    }
}