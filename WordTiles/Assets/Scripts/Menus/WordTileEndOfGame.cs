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
    /// Duration to move the streamers
    /// </summary>
    private const float STREAMER_LERP_DURATION = 1f;

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
    /// <summary>
    /// Label that will display the score.
    /// </summary>
    [SerializeField]
    [Tooltip("Label used to display the final score.")]
    private Text m_scoreLabel;

    /// <summary>
    /// Button used to restart game
    /// </summary>
    [SerializeField]
    [Tooltip("Button to restart the game")]
    private Button m_restartButton;

    /// <summary>
    /// Streamers for celebration
    /// </summary>
    [SerializeField]
    [Tooltip("Celebration streamers.")]
    private Image m_streamers;
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

        //Animate the streamers
        StartCoroutine(MoveStreamersCo());
    }

    /// <summary>
    /// Callback called when the restart button is pressed
    /// </summary>
    private void OnRestartButtonPressed()
    {
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartGame>();
        GameObject.Destroy(this.gameObject);
    }

    /// <summary>
    /// Moves the streamers on screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveStreamersCo()
    {
        float currDuration = 0f;
        Vector3 targetPos = new Vector3(0f, m_streamers.rectTransform.rect.size.y, 0f);
        while (currDuration < STREAMER_LERP_DURATION)
        {
            currDuration += Time.deltaTime;
            m_streamers.rectTransform.localPosition = Vector3.Slerp(Vector3.zero, targetPos, (currDuration / STREAMER_LERP_DURATION));
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        currDuration = 0f;
        while (currDuration < STREAMER_LERP_DURATION)
        {
            currDuration += Time.deltaTime;
            m_streamers.rectTransform.localPosition = Vector3.Slerp(targetPos, Vector3.zero, (currDuration / STREAMER_LERP_DURATION));
            yield return null;
        }
    }
    #endregion PrivateMethods
}
