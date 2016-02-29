using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public sealed class WordTilesMainMenu : MonoBehaviour
{
    public const string PATH_TO_MENU = "Prefabs/WordTiles_MainMenu";

    public const int NUMBER_OF_PLAYABLE_TILES = 8;

    //Bottom of the bar
    [SerializeField]
    private Text m_currentRoundLabel;

    [SerializeField]
    private GameObject m_submitButton;

    [SerializeField]
    private WordTileTray m_wordTray;

    public WordTileTray GetWordTileTray()
    {
        return m_wordTray;
    }
}