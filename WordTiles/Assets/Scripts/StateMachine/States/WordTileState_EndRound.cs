using UnityEngine;
using System.Collections;

public sealed class WordTileState_EndRound : State
{
    /// <summary>
    /// Checks if the game has ended or if we should continue playing another round.
    /// </summary>
    /// <param name="argArguments">State arguments.</param>
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        //If the round that just ended was the last round, then end the game.
        if (WordTilesGameManager.Instance.IsOnLastRound() == true)
        {
            WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_EndGame>();
        }
        else
        {
            WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartRound>();
        }
    }
}
