using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartRound : State
{
    /// <summary>
    /// Starts a new round. Shuffle in new tiles and resets the slots.
    /// </summary>
    /// <param name="argArguments">State arguments.</param>
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        //Increment the round number we are currently on
        WordTilesGameManager.Instance.IncrementRound();

        //Detach any tiles that may be on the slots
        WordTileSlotGroup slotGroup = WordTilesGameManager.Instance.MainMenuScreen.GetWordTileSlotGroup();
        slotGroup.Reset();

        //Disable buttons before the shuffle animation
        WordTilesGameManager.Instance.MainMenuScreen.EnableButtons(false);

        //Now play the animation to play in thew new tiles. Wait for the animation to complete before moving to the next state.
        WordTileTray tray = WordTilesGameManager.Instance.MainMenuScreen.GetWordTileTray();
        tray.ShuffleInNewTiles(OnTrayShuffleComplete);
    }

    private void OnTrayShuffleComplete()
    {
        //Re-enable the buttons
        WordTilesGameManager.Instance.MainMenuScreen.EnableButtons(true);

        //Now that the round has started, move to the run state.
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_RunRound>();
    }
}
