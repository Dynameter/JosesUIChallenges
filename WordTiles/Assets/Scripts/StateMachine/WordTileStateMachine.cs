using UnityEngine;

/// <summary>
/// A monobehaviour wrapper for the state machine.
/// </summary>
public sealed class WordTileStateMachine : MonoBehaviour
{
    private static WordTileStateMachine _instance;

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

    private WordTilesMainMenu m_mainMenu;
    public WordTilesMainMenu MainMenuScreen
    {
        get
        {
            return m_mainMenu;
        }

        set
        {
            m_mainMenu = value;
            m_mainMenu.GetComponent<RectTransform>().SetParent(this.transform, false);
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