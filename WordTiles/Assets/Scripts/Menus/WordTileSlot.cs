using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class WordTileSlot : MonoBehaviour, IDropHandler
{
    #region PrivateMembers
    /// <summary>
    /// The tile that is attached to the slot.
    /// </summary>
    private WordTile m_attachedTile;
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Attached the word tile to the slot.
    /// </summary>
    /// <param name="argTile">The tile to attach.</param>
    private void SnapTileToSlot(WordTile argTile)
    {
        m_attachedTile = argTile;
        m_attachedTile.SetGrandParent(this.transform);

        RectTransform tileParent = argTile.GetParent();
        tileParent.position = this.transform.position;

        m_attachedTile.SetOnDetachedCallback(DetachTile);
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Handles a drop event.
    /// </summary>
    /// <param name="eventData">Drop event data.</param>
    public void OnDrop(PointerEventData eventData)
    {
        //Check if what was dropped was a tile
        WordTile tile = eventData.pointerDrag.GetComponentInChildren<WordTile>();
        if (tile != m_attachedTile && tile != null && tile.IsInteractable() == true)
        {
            //If there was a previously attached tile, return it to the tray.
            if (m_attachedTile != null)
            {
                WordTile attachedTile = m_attachedTile;
                WordTilesGameManager.Instance.GetMainMenu().GetWordTileTray().ReturnTile(attachedTile);
            }

            SnapTileToSlot(tile);
        }
    }

    /// <summary>
    /// Detaches any tiles on this slot.
    /// </summary>
    public void DetachTile()
    {
        DetachTile(m_attachedTile);
    }

    /// <summary>
    /// Detaches any tiles on this slot.
    /// </summary>
    /// <param name="argDetachedTile">The tile requested to be removed.</param>
    public void DetachTile(WordTile argDetachedTile)
    {
        if (m_attachedTile == argDetachedTile)
        {
            m_attachedTile = null;
        }
    }

    /// <summary>
    /// Gets the attached word tile.
    /// </summary>
    /// <returns>Returns the attached word tile.</returns>
    public WordTile GetAttachedWordTile()
    {
        return m_attachedTile;
    }
    #endregion PublicMethods
}
