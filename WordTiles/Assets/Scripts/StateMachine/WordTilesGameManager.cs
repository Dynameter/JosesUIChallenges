using System.IO;
using System.Collections;

using UnityEngine;

public sealed class WordTilesGameManager
{
    #region StaticMembers
    /// <summary>
    /// Method signature for a game manager value update callback.
    /// </summary>
    /// <param name="argNewValue">The updated value that was changed.</param>
    public delegate void OnGameManagerValueChanged(uint argNewValue);

    /// <summary>
    /// How many rounds can we play before the game ends.
    /// </summary>
    public const uint MAX_ROUNDS = 1;

    /// <summary>
    /// The number of tiles that go in the tray.
    /// </summary>
    public const int NUMBER_OF_PLAYABLE_TILES = 8;

    /// <summary>
    /// Path to the words file
    /// </summary>
    public const string PATH_TO_WORDS_FILE = "Words/AllWords";

    /// <summary>
    /// The singleton instance of the game manager.
    /// </summary>
    private static WordTilesGameManager _instance;

    /// <summary>
    /// The singleton instance of the game manager.
    /// </summary>
    public static WordTilesGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WordTilesGameManager();
            }

            return _instance;
        }
    }
    #endregion StaticMembers

    #region PrivateMembers
    /// <summary>
    /// Accumulated score.
    /// </summary>
    private uint m_score;

    /// <summary>
    /// Event called when the score changes.
    /// </summary>
    public event OnGameManagerValueChanged OnScoreChanged;

    /// <summary>
    /// Current round the player is playing on.
    /// </summary>
    private uint m_currentRound;

    /// <summary>
    /// Main menu instance.
    /// </summary>
    private WordTilesMainMenu m_mainMenu;

    /// <summary>
    /// Tree that contains all playable words.
    /// </summary>
    private readonly DictionaryTree m_dictionaryTree;

    /// <summary>
    /// Event called when the round changes.
    /// </summary>
    public event OnGameManagerValueChanged OnCurrentRoundChanged;
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Constructs and initialized the game manager.
    /// </summary>
    private WordTilesGameManager()
    {
        m_dictionaryTree = new DictionaryTree();
        PopulateWords(Resources.Load<TextAsset>(PATH_TO_WORDS_FILE));

        Reset();
    }

    /// <summary>
    /// Populates the word tree with all of the playable words.
    /// </summary>
    /// <param name="argWordAsset">The file that contains all of the playable words.</param>
    private void PopulateWords(TextAsset argWordAsset)
    {
        using (MemoryStream fileReader = new MemoryStream(argWordAsset.bytes))
        {
            using (StreamReader streamReader = new StreamReader(fileReader))
            {
                while (streamReader.EndOfStream == false)
                {
                    m_dictionaryTree.AddWord(streamReader.ReadLine());
                }
            }
        }
    }

    private void SetRound(uint argRound)
    {
        m_currentRound = argRound;

        if (OnCurrentRoundChanged != null)
        {
            OnCurrentRoundChanged(m_currentRound);
        }
    }

    private void SetScore(uint argScore)
    {
        m_score = argScore;

        if (OnScoreChanged != null)
        {
            OnScoreChanged(m_score);
        }
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Resets the round and score back to zero.
    /// </summary>
    public void Reset()
    {
        SetScore(0);
        SetRound(0);

        if (m_mainMenu != null)
        {
            m_mainMenu.Reset();
        }
    }

    /// <summary>
    /// Gets the current score.
    /// </summary>
    /// <returns>Returns the current score.</returns>
    public uint GetScore()
    {
        return m_score;
    }

    /// <summary>
    /// Increment the round we are on.
    /// </summary>
    public void IncrementRound()
    {
        SetRound(m_currentRound + 1);
    }

    /// <summary>
    /// Checks to see if the game is on the last playable round.
    /// </summary>
    /// <returns>Returns true if on the last playable round.</returns>
    public bool IsOnLastRound()
    {
        return (m_currentRound == MAX_ROUNDS);
    }

    /// <summary>
    /// Adds the value to the current score.
    /// </summary>
    /// <param name="argAmountToAdd">The amount to add to the score.</param>
    public void AddToScore(uint argAmountToAdd)
    {
        SetScore(m_score + argAmountToAdd);
    }

    /// <summary>
    /// Gets the main menu.
    /// </summary>
    /// <returns>Returns the active instance of the main menu.</returns>
    public WordTilesMainMenu GetMainMenu()
    {
        return m_mainMenu;
    }

    /// <summary>
    /// Sets the active instance of the main menu.
    /// </summary>
    /// <param name="argMainMenu">The instantiated main menu.</param>
    public void SetMainMenu(WordTilesMainMenu argMainMenu)
    {
        m_mainMenu = argMainMenu;
    }

    /// <summary>
    /// Checks to see if the given string is a playable word.
    /// </summary>
    /// <param name="argWord">The string to check.</param>
    /// <returns>Returns true if the string is a playable word.</returns>
    public bool DoesWordExist(string argWord)
    {
        return m_dictionaryTree.DoesWordExist(argWord);
    }
    #endregion PublicMethods
}
