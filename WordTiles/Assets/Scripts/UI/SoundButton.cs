using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SoundButton : Button
{
    #region StaticMembers
    /// <summary>
    /// Path to the tile picked up sound.
    /// </summary>
    private const string PATH_TO_CLICK_ONE_SOUND = "Sounds/click0";

    /// <summary>
    /// Audio clip to play when we pick up the tile.
    /// </summary>
    private static AudioClip _clickOne;

    /// <summary>
    /// Audio clip to play when we pick up the tile.
    /// </summary>
    public static AudioClip ClickOne
    {
        get
        {
            if(_clickOne == null)
            {
                _clickOne = Resources.Load<AudioClip>(PATH_TO_CLICK_ONE_SOUND);
            }

            return _clickOne;
        }
    }

    /// <summary>
    /// Path to the tile dropped sound.
    /// </summary>
    private const string PATH_TO_CLICK_TWO_PATH = "Sounds/click1";

    /// <summary>
    /// Audio clip to play when the tile is dropped.
    /// </summary>
    private static AudioClip _clickTwo;

    /// <summary>
    /// Audio clip to play when the tile is dropped.
    /// </summary>
    public static AudioClip ClickTwo
    {
        get
        {
            if(_clickTwo == null)
            {
                _clickTwo = Resources.Load<AudioClip>(PATH_TO_CLICK_TWO_PATH);
            }

            return _clickTwo;
        }
    }
    #endregion StaticMembers

    #region PublicMethods
    /// <summary>
    /// Event raised when the mouse/finger is down on the button.
    /// </summary>
    /// <param name="eventData">Pointer down event data.</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsInteractable() == true)
        {
            AudioManager.Instance.PlaySound(ClickOne);
        }
    }

    /// <summary>
    /// Event raised when the mouse/finger is raised from the button.
    /// </summary>
    /// <param name="eventData">Pointer up event data.</param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (IsInteractable() == true)
        {
            AudioManager.Instance.PlaySound(ClickTwo);
        }
    }
    #endregion PublicMethods
}
