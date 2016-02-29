using UnityEngine;
using UnityEngine.UI;

public sealed class WordTileHowToPlay : MonoBehaviour
{
    public const string PATH_TO_MENU = "Prefabs/WordTiles_HowToPlay";

    [SerializeField]
    private Button m_okButton;

    private void Start()
    {
        m_okButton.onClick.AddListener(OnOKButtonPressed);
    }

    private void OnOKButtonPressed()
    {
        //Destroy this menu
        GameObject.Destroy(this.gameObject);

        //Transition to another state
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartGame>();
    }
}
