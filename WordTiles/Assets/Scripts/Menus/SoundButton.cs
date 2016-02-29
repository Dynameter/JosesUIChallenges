using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SoundButton : Button
{
    /// <summary>
    /// Path to the tile picked up sound.
    /// </summary>
    private const string CLICK_ONE_SOUND_PATH = "Sounds/click0";

    /// <summary>
    /// Audio clip to play when we pick up the tile.
    /// </summary>
    private static AudioClip _clickOne;

    /// <summary>
    /// Audio clip to play when we pick up the tile.
    /// </summary>
    public static AudioClip CLICK_ONE_SOUND
    {
        get
        {
            if(_clickOne == null)
            {
                _clickOne = Resources.Load<AudioClip>(CLICK_ONE_SOUND_PATH);
            }

            return _clickOne;
        }
    }

    /// <summary>
    /// Path to the tile dropped sound.
    /// </summary>
    private const string CLICK_TWO_SOUND_PATH = "Sounds/click1";

    /// <summary>
    /// Audio clip to play when the tile is dropped.
    /// </summary>
    private static AudioClip _clickTwo;

    /// <summary>
    /// Audio clip to play when the tile is dropped.
    /// </summary>
    public static AudioClip CLICK_TWO
    {
        get
        {
            if(_clickTwo == null)
            {
                _clickTwo = Resources.Load<AudioClip>(CLICK_TWO_SOUND_PATH);
            }

            return _clickTwo;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        AudioManager.Instance.PlaySound(CLICK_ONE_SOUND);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        AudioManager.Instance.PlaySound(CLICK_TWO);
    }
}
