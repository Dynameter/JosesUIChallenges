using UnityEngine;
using UnityEngine.UI;

public sealed class WordTileHowToPlay : MonoBehaviour
{
    /// <summary>
    /// Path to the popup prefab.
    /// </summary>
    public const string PATH_TO_HOW_TO_PLAY_MENU = "Prefabs/WordTiles_HowToPlay";
    
    /// <summary>
    /// Button that closes the popup.
    /// </summary>
    [SerializeField]
    [Tooltip("OK button to close the popup")]
    private Button m_okButton;

    /// <summary>
    /// Initialized the popup.
    /// </summary>
    private void Start()
    {
        m_okButton.onClick.AddListener(OnOKButtonPressed);
    }

    /// <summary>
    /// Callback called when the OK button is pressed.
    /// </summary>
    private void OnOKButtonPressed()
    {
        //Destroy this menu
        GameObject.Destroy(this.gameObject);

        //Transition to another state
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartGame>();
    }
}
