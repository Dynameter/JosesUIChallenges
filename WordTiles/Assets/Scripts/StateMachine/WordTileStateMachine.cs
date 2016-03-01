using UnityEngine;

/// <summary>
/// A monobehaviour wrapper for the state machine.
/// </summary>
public sealed class WordTileStateMachine : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the state machine.
    /// </summary>
    private static WordTileStateMachine _instance;

    /// <summary>
    /// Singleton instance of the state machine.
    /// </summary>
    public static WordTileStateMachine Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// The state machine instance.
    /// </summary>
    private StateMachine m_stateMachine = new StateMachine();

    /// <summary>
    /// The state machine instance.
    /// </summary>
    public StateMachine SM
    {
        get
        {
            return m_stateMachine;
        }
    }

    /// <summary>
    /// Sets the starting state
    /// </summary>
    public void Start()
    {
        _instance = this;
        m_stateMachine.SetCurrentStateTo<WordTileState_Init>();
    }

    /// <summary>
    /// Calls update on the state machine.
    /// </summary>
    public void Update()
    {
        m_stateMachine.UpdateState();
    }
}