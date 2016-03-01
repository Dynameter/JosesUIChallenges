using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WordTileState_Init : State
{
    /// <summary>
    /// Initializes the game.
    /// </summary>
    /// <param name="argArguments">State arguments.</param>
    public override void Enter(Dictionary<string, object> argArguments)
    {
        base.Enter(argArguments);

        //Create the main menu and the how to play screens for future use
        WordTilesGameManager.Instance.MainMenuScreen = WordTilesMainMenu.Instantiate<WordTilesMainMenu>(Resources.Load<WordTilesMainMenu>(WordTilesMainMenu.PATH_TO_MAIN_MENU));
        WordTilesGameManager.Instance.MainMenuScreen.GetComponent<RectTransform>().SetParent(WordTileStateMachine.Instance.transform, false);

        //Switch to th next state to show the how to play screen
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_ShowHowToPlay>();
    }
}
