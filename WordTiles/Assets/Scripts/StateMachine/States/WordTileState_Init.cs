using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WordTileState_Init : State
{
    /// <summary>
    /// Initializes the game.
    /// </summary>
    public override void Enter()
    {
        //Create the main menu and the how to play screens for future use
        WordTilesMainMenu mainMenuInstance = WordTilesMainMenu.Instantiate<WordTilesMainMenu>(Resources.Load<WordTilesMainMenu>(WordTilesMainMenu.PATH_TO_MAIN_MENU));
        WordTilesGameManager.Instance.SetMainMenu(mainMenuInstance);
        mainMenuInstance.GetComponent<RectTransform>().SetParent(WordTileStateMachine.Instance.transform, false);

        //Switch to th next state to show the how to play screen
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_ShowHowToPlay>();
    }
}
