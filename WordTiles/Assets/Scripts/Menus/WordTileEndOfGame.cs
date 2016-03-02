using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public sealed class WordTileEndOfGame : MonoBehaviour
{
    #region StaticMembers
    /// <summary>
    /// Path to the popup prefab.
    /// </summary>
    public const string PATH_TO_END_OF_GAME_MENU = "Prefabs/WordTiles_EndOfGame";

    /// <summary>
    /// Path to the cheering sound.
    /// </summary>
    private const string PATH_TO_CHEERING_SOUND = "Sounds/cheering";

    /// <summary>
    /// Cheering sound for game over.
    /// </summary>
    private static AudioClip _cheeringSound;

    /// <summary>
    /// Cheering sound for game over.
    /// </summary>
    public static AudioClip CheeringSound
    {
        get
        {
            if (_cheeringSound == null)
            {
                _cheeringSound = Resources.Load<AudioClip>(PATH_TO_CHEERING_SOUND);
            }

            return _cheeringSound;
        }
    }
    #endregion StaticMembers

    #region PrivateMembers
    [SerializeField]
    [Tooltip("Label used to display the final score.")]
    private Text m_scoreLabel;

    /// <summary>
    /// Button used to restart game
    /// </summary>
    [SerializeField]
    [Tooltip("Button to restart the game")]
    private Button m_restartButton;
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Initializes the menu
    /// </summary>
    private void Awake()
    {
        //Set the score
        m_scoreLabel.text = WordTilesGameManager.Instance.GetScore().ToString();

        //Set up button callback
        m_restartButton.onClick.AddListener(OnRestartButtonPressed);

        //Play the cheering sound
        AudioManager.Instance.PlaySound(CheeringSound);
    }

    /// <summary>
    /// Callback called when the restart button is pressed
    /// </summary>
    private void OnRestartButtonPressed()
    {
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartGame>();
        GameObject.Destroy(this.gameObject);
    }
    #endregion PrivateMethods
}
