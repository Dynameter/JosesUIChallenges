using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartGame : State
{
    /// <summary>
    /// Starts a new game.
    /// </summary>
    public override void Enter()
    {
        //Initialize the game manager
        WordTilesGameManager.Instance.Reset();

        //Start a new round
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartRound>();
    }
}
