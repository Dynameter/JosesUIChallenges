using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartGame : State
{
    /// <summary>
    /// Starts a new game.
    /// </summary>
    /// <param name="argArguments">State arguments.</param>
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        //Initialize the game manager
        WordTilesGameManager.Instance.Reset();

        //Start a new round
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartRound>();
    }
}
