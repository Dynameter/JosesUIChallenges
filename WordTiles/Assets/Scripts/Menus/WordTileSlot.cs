using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class WordTileSlot : MonoBehaviour, IDropHandler
{
    private WordTile m_attachedTile;

    public void OnDrop(PointerEventData eventData)
    {
        if (m_attachedTile == null)
        {
            //Check if what was dropped was a tile
            WordTile tile = eventData.pointerDrag.GetComponentInChildren<WordTile>();
            if (tile != null && tile.IsInteractable() == true)
            {
                SnapTileToSlot(tile);
            }
        }
    }

    private void SnapTileToSlot(WordTile argTile)
    {
        m_attachedTile = argTile;
        RectTransform tileParent = argTile.GetParent();
        tileParent.SetParent(this.transform);
        tileParent.position = this.transform.position;
    }
}
