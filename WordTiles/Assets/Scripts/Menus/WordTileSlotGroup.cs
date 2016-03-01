using UnityEngine;
using System.Collections;

public sealed class WordTileSlotGroup : MonoBehaviour
{
    /// <summary>
    /// Slots that belong to this group.
    /// </summary>
    [SerializeField]
    private WordTileSlot[] m_slots = new WordTileSlot[WordTilesGameManager.NUMBER_OF_PLAYABLE_TILES];

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
}
