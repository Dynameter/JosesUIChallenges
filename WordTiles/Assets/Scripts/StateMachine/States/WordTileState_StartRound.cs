using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartRound : State
{
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        WordTileTray tray =  WordTileStateMachine.Instance.MainMenuScreen.GetWordTileTray();
        tray.ShuffleInNewTiles(OnTrayShuffleComplete);
    }

    private void OnTrayShuffleComplete()
    {
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_RunRound>();
    }
}
