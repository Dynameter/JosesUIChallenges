using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WordTileState_Init : State
{
    public override void Enter(Dictionary<string, object> argArguments)
    {
        base.Enter(argArguments);

        //Create the main menu and the how to play screens for future use
        WordTileStateMachine.Instance.MainMenuScreen = WordTilesMainMenu.Instantiate<WordTilesMainMenu>(Resources.Load<WordTilesMainMenu>(WordTilesMainMenu.PATH_TO_MENU));

        //Switch to th next state to show the how to play screen
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_ShowHowToPlay>();
    }
}
