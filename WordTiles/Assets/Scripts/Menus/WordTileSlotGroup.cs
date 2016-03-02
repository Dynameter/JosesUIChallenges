using UnityEngine;
using System.Collections;

public sealed class WordTileSlotGroup : MonoBehaviour
{
    #region StaticMembers
    /// <summary>
    /// Method signature for a callback to call when all of the word trays have finished popping.
    /// </summary>
    public delegate void OnWordTilesPopped();

    /// <summary>
    /// The target scale when "popping" a tile.
    /// </summary>
    private static readonly Vector3 POPPED_TILE_SCALE = new Vector3(1.2f, 1.2f, 1f);

    /// <summary>
    /// Path to the audio clip for the popping sound.
    /// </summary>
    private const string PATH_TO_POPPING_SOUND = "Sounds/bubble";

    /// <summary>
    /// Audio clip for the popping sound.
    /// </summary>
    private AudioClip _poppingSound;

    /// <summary>
    /// Audio clip for the popping sound.
    /// </summary>
    private AudioClip PoppingSound
    {
        get
        {
            if (_poppingSound == null)
            {
                _poppingSound = Resources.Load<AudioClip>(PATH_TO_POPPING_SOUND);
            }

            return _poppingSound;
        }
    }
    #endregion

    #region PrivateMembers
    /// <summary>
    /// Slots that belong to this group.
    /// </summary>
    [SerializeField]
    private WordTileSlot[] m_slots = new WordTileSlot[WordTilesGameManager.NUMBER_OF_PLAYABLE_TILES];
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// "Pops" all tiles one by one.
    /// </summary>
    /// <param name="argOnPopped">The callback to call when the tiles have finished popping.</param>
    /// <returns>Returns the enumerator to </returns>
    private IEnumerator PopTilesCo(OnWordTilesPopped argOnPopped)
    {
        float lerpDuration = (PoppingSound.length / 1.5f);
        for (int i = 0; i < m_slots.Length; ++i)
        {
            WordTile tile = m_slots[i].GetAttachedWordTile();
            if (tile != null)
            {
                //Play the popping sound
                AudioManager.Instance.PlaySound(PoppingSound);

                float currDuration = 0f;
                while (currDuration < lerpDuration)
                {
                    currDuration += Time.deltaTime;
                    tile.GetRectTransform().localScale = Vector3.Lerp(Vector3.one, POPPED_TILE_SCALE, (currDuration / lerpDuration));

                    yield return null;
                }

                //Hide the tile
                tile.gameObject.SetActive(false);
            }
            else
            {
                break;
            }
        }

        //Add a little delay
        yield return new WaitForSeconds(lerpDuration);

        //If there is a callback, call it.
        if (argOnPopped != null)
        {
            argOnPopped();
        }
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Detaches all word tiles from the slots.
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < m_slots.Length; ++i)
        {
            m_slots[i].DetachTile();
        }
    }

    /// <summary>
    /// Checks to see if the current slot/tile configuration is playable.
    /// </summary>
    /// <returns>Returns true if the slots are in a playable state.</returns>
    public bool AreSlotsPlayable()
    {
        bool isPlayable = false;
        for (int i = m_slots.Length - 1; i >= 0; --i)
        {
            WordTile wordTile = m_slots[i].GetAttachedWordTile();
            if (isPlayable && wordTile == null)
            {
                return false;
            }
            else if (wordTile != null)
            {
                isPlayable = true;
            }
        }

        return isPlayable;
    }

    /// <summary>
    /// Checks to see if all of the slots are empty.
    /// </summary>
    /// <returns>Returns true is all of the slots are empty.</returns>
    public bool AreSlotsEmpty()
    {
        for (int i = 0; i < m_slots.Length; ++i)
        {
            if (m_slots[i].GetAttachedWordTile() != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Constructs the word formed from the tiles.
    /// </summary>
    /// <returns>Returns the string formed by the tile characters.</returns>
    public string GetPlayableWord()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < m_slots.Length; ++i)
        {
            WordTile wordTile = m_slots[i].GetAttachedWordTile();
            if (wordTile != null)
            {
                sb.Append(wordTile.GetLetter());
            }
            else
            {
                break;
            }
        }

        return sb.ToString().ToLower();
    }

    /// <summary>
    /// Pop all tiles that are on the slots.
    /// </summary>
    /// <param name="argOnPopped">The callback to call when all the tiles are popped.</param>
    public void PopTiles(OnWordTilesPopped argOnPopped)
    {
        StartCoroutine(PopTilesCo(argOnPopped));
    }
    #endregion PublicMethods
}
