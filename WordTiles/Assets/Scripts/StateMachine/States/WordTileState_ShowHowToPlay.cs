using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WordTileState_ShowHowToPlay : State
{
    /// <summary>
    /// Displays the how to play menu.
    /// </summary>
    /// <param name="argArguments">State arguments.</param>
    public override void Enter(Dictionary<string, object> argArguments)
    {
        //Show the how to play screen and then wait until the "ok" button has been clicked before going to the next state.
        WordTileHowToPlay howToPlayScreen = WordTileHowToPlay.Instantiate<WordTileHowToPlay>(Resources.Load<WordTileHowToPlay>(WordTileHowToPlay.PATH_TO_HOW_TO_PLAY_MENU));
        howToPlayScreen.GetComponent<RectTransform>().SetParent(WordTileStateMachine.Instance.transform, false);
    }
}
