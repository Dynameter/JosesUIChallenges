using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public sealed class WordTilesMainMenu : MonoBehaviour
{
    /// <summary>
    /// Path to the prefab
    /// </summary>
    public const string PATH_TO_MAIN_MENU = "Prefabs/WordTiles_MainMenu";

    /// <summary>
    /// Path to the valid word sound.
    /// </summary>
    public const string PATH_TO_WORD_SUBMIT_SOUND = "Sounds/select-classic";

    /// <summary>
    /// Path to the invalid word sound.
    /// </summary>
    public const string PATH_TO_INVALID_WORD_SUBMIT_SOUND = "Sounds/notice";

    /// <summary>
    /// The label that displays the current round.
    /// </summary>
    [SerializeField]
    [Tooltip("Label that displays the current round")]
    private Text m_currentRoundLabel;

    /// <summary>
    /// The label that displays the current score.
    /// </summary>
    [SerializeField]
    [Tooltip("Label that displays the current score")]
    private Text m_scoreLabel;

    /// <summary>
    /// Button used to submit the word formed by the tiles in the slots.
    /// </summary>
    [SerializeField]
    [Tooltip("Button used to submit the formed word for a score")]
    private Button m_submitButton;

    /// <summary>
    /// Button use to reshuffle the tiles.
    /// </summary>
    [SerializeField]
    [Tooltip("Button used to shuffle the letters")]
    private Button m_shuffleButton;

    /// <summary>
    /// The group of tile slots.
    /// </summary>
    [SerializeField]
    [Tooltip("Group that contains all of the tile slots")]
    private WordTileSlotGroup m_slotGroup;

    /// <summary>
    /// The word tray that contains all of the tiles.
    /// </summary>
    [SerializeField]
    [Tooltip("Tray that contains all of the playable tiles")]
    private WordTileTray m_wordTray;

    /// <summary>
    /// Sound played when a valid word is submitted.
    /// </summary>
    private static AudioClip _wordSubmittedSound;

    /// <summary>
    /// Sound played when a valid word is submitted.
    /// </summary>
    public static AudioClip WordSubmittedSound
    {
        get
        {
            if (_wordSubmittedSound == null)
            {
                _wordSubmittedSound = Resources.Load<AudioClip>(PATH_TO_WORD_SUBMIT_SOUND);
            }

            return _wordSubmittedSound;
        }
    }

    /// <summary>
    /// Sound played when an invalid word is submitted.
    /// </summary>
    private static AudioClip _invalidWordSubmittedSound;

    /// <summary>
    /// Sound played when an invalid word is submitted.
    /// </summary>
    public static AudioClip InvalidWordSubmittedSound
    {
        get
        {
            if (_invalidWordSubmittedSound == null)
            {
                _invalidWordSubmittedSound = Resources.Load<AudioClip>(PATH_TO_INVALID_WORD_SUBMIT_SOUND);
            }

            return _invalidWordSubmittedSound;
        }
    }

    /// <summary>
    /// Initialized the main menu.
    /// </summary>
    public void Awake()
    {
        WordTilesGameManager.Instance.OnScoreChanged += OnScoreChanged;
        WordTilesGameManager.Instance.OnCurrentRoundChanged += OnRoundChanged;

        m_submitButton.onClick.AddListener(OnSubmitButtonPressed);
        m_shuffleButton.onClick.AddListener(OnShuffleButtonPressed);
    }

    /// <summary>
    /// Resets the slot group and word tile tray.
    /// </summary>
    public void Reset()
    {
        m_slotGroup.Reset();
        m_wordTray.Reset();
    }

    /// <summary>
    /// Gets the word tile slot group.
    /// </summary>
    /// <returns>Returns the word tile slot group.</returns>
    public WordTileSlotGroup GetWordTileSlotGroup()
    {
        return m_slotGroup;
    }

    /// <summary>
    /// Gets the word tile tray instance.
    /// </summary>
    /// <returns>The word tile tray instance attached to this menu</returns>
    public WordTileTray GetWordTileTray()
    {
        return m_wordTray;
    }

    /// <summary>
    /// Callback called when the score has been updated.
    /// </summary>
    /// <param name="argScore">The updated score.</param>
    private void OnScoreChanged(uint argScore)
    {
        m_scoreLabel.text = ("SCORE: " + argScore.ToString());
    }

    /// <summary>
    /// Callback called when the round has been updated.
    /// </summary>
    /// <param name="argRound">The updated round.</param>
    private void OnRoundChanged(uint argRound)
    {
        m_currentRoundLabel.text = ("ROUND: " + argRound.ToString() + "/" + WordTilesGameManager.MAX_ROUNDS.ToString());
    }

    /// <summary>
    /// Callback called when the submit button is pressed.
    /// </summary>
    private void OnSubmitButtonPressed()
    {
        //Check to see if we have a valid word
        string tileWord = this.m_slotGroup.GetPlayableWord();
        if (string.IsNullOrEmpty(tileWord) == false && WordTilesGameManager.Instance.DoesWordExist(tileWord))
        {
            //Play the submit sound
            AudioManager.Instance.PlaySound(WordSubmittedSound);

            //Add to the score
            WordTilesGameManager.Instance.AddToScore((uint)tileWord.Length);

            WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_EndRound>();
        }
        else
        {
            //Play the invalid sound.
            AudioManager.Instance.PlaySound(InvalidWordSubmittedSound);
        }
    }

    /// <summary>
    /// Callback called when the shuffle button is pressed.
    /// </summary>
    private void OnShuffleButtonPressed()
    {
        if (m_wordTray.IsShuffling() == false)
        {
            m_slotGroup.Reset();
            m_wordTray.ShuffleInNewTiles(null);
        }
    }

    /// <summary>
    /// Enables or disables the buttons.
    /// </summary>
    /// <param name="argEnable">Wether to enable or disable the buttons.</param>
    public void EnableButtons(bool argEnable)
    {
        m_submitButton.interactable = argEnable;
        m_shuffleButton.interactable = argEnable;
    }
}