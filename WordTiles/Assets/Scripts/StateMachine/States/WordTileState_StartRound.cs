using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartRound : State
{
    /// <summary>
    /// Starts a new round. Shuffle in new tiles and resets the slots.
    /// </summary>
    public override void Enter()
    {
        //Increment the round number we are currently on
        WordTilesGameManager.Instance.IncrementRound();

        //Detach any tiles that may be on the slots
        WordTileSlotGroup slotGroup = WordTilesGameManager.Instance.GetMainMenu().GetWordTileSlotGroup();
        slotGroup.Reset();

        //Disable buttons before the shuffle animation
        WordTilesGameManager.Instance.GetMainMenu().EnableButtons(false);

        //Now play the animation to play in thew new tiles. Wait for the animation to complete before moving to the next state.
        WordTileTray tray = WordTilesGameManager.Instance.GetMainMenu().GetWordTileTray();
        tray.ShuffleInNewTiles(OnTrayShuffleComplete);
    }

    private void OnTrayShuffleComplete()
    {
        //Re-enable the buttons
        WordTilesGameManager.Instance.GetMainMenu().EnableButtons(true);

        //Now that the round has started, move to the run state.
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_RunRound>();
    }
}
