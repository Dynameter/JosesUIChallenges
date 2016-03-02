using UnityEngine;
using System.Collections;

public sealed class WordTileState_EndGame : State
{
    /// <summary>
    /// Displays the game over screen
    /// </summary>
    public override void Enter()
    {
        WordTileEndOfGame endOfGamePopup = WordTileEndOfGame.Instantiate<WordTileEndOfGame>(Resources.Load<WordTileEndOfGame>(WordTileEndOfGame.PATH_TO_END_OF_GAME_MENU));
        endOfGamePopup.GetComponent<RectTransform>().SetParent(WordTileStateMachine.Instance.transform, false);
    }
}
