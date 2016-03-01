using System.IO;
using System.Collections;

using UnityEngine;

public sealed class WordTilesGameManager
{
    /// <summary>
    /// Method signature for a game manager value update callback.
    /// </summary>
    /// <param name="argNewValue">The updated value that was changed.</param>
    public delegate void OnGameManagerValueChanged(uint argNewValue);

    /// <summary>
    /// How many rounds can we play before the game ends.
    /// </summary>
    public const uint MAX_ROUNDS = 10;

    /// <summary>
    /// The number of tiles that go in the tray.
    /// </summary>
    public const int NUMBER_OF_PLAYABLE_TILES = 8;

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

    /// <summary>
    /// Accumulated score.
    /// </summary>
    private uint m_score;

    /// <summary>
    /// Accumulated score.
    /// </summary>
    public uint Score
    {
        get
        {
            return m_score;
        }

        private set
        {
            m_score = value;

            if (OnScoreChanged != null)
            {
                OnScoreChanged(m_score);
            }
        }
    }

    /// <summary>
    /// Event called when the score changes.
    /// </summary>
    public event OnGameManagerValueChanged OnScoreChanged;

    /// <summary>
    /// Current round the player is playing on.
    /// </summary>
    private uint m_currentRound;

    /// <summary>
    /// Current round the player is playing on.
    /// </summary>
    public uint CurrentRound
    {
        get
        {
            return m_currentRound;
        }

        private set
        {
            m_currentRound = value;

            if (OnCurrentRoundChanged != null)
            {
                OnCurrentRoundChanged(m_currentRound);
            }
        }
    }

    /// <summary>
    /// Main menu instance.
    /// </summary>
    private WordTilesMainMenu m_mainMenu;

    /// <summary>
    /// Main menu instance.
    /// </summary>
    public WordTilesMainMenu MainMenuScreen
    {
        get
        {
            return m_mainMenu;
        }

        set
        {
            m_mainMenu = value;
        }
    }

    /// <summary>
    /// Tree that contains all playable words.
    /// </summary>
    private readonly DictionaryTree m_dictionaryTree;

    /// <summary>
    /// Event called when the round changes.
    /// </summary>
    public event OnGameManagerValueChanged OnCurrentRoundChanged;

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

    /// <summary>
    /// Checks to see if the given string is a playable word.
    /// </summary>
    /// <param name="argWord">The string to check.</param>
    /// <returns>Returns true if the string is a playable word.</returns>
    public bool DoesWordExist(string argWord)
    {
        return m_dictionaryTree.DoesWordExist(argWord);
    }

    /// <summary>
    /// Resets the round and score back to zero.
    /// </summary>
    public void Reset()
    {
        Score = 0;
        CurrentRound = 0;

        if (MainMenuScreen != null)
        {
            MainMenuScreen.Reset();
        }
    }

    /// <summary>
    /// Increment the round we are on.
    /// </summary>
    public void IncrementRound()
    {
        ++CurrentRound;
    }

    /// <summary>
    /// Checks to see if the game is on the last playable round.
    /// </summary>
    /// <returns>Returns true if on the last playable round.</returns>
    public bool IsOnLastRound()
    {
        return (CurrentRound == MAX_ROUNDS);
    }

    /// <summary>
    /// Adds the value to the current score.
    /// </summary>
    /// <param name="argAmountToAdd">The amount to add to the score.</param>
    public void AddToScore(uint argAmountToAdd)
    {
        Score += argAmountToAdd;
    }
}
