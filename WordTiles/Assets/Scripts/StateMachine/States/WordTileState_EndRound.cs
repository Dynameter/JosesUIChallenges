using UnityEngine;
using System.Collections;

public sealed class WordTileState_EndRound : State
{
    /// <summary>
    /// Checks if the game has ended or if we should continue playing another round.
    /// </summary>
    public override void Enter()
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
