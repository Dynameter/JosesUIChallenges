using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class WordTileSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Check if what was dropped was a tile
        WordTile tile = eventData.pointerDrag.GetComponentInChildren<WordTile>();
        if (tile != null)
        {
            SnapTileToSlot(tile);
        }
    }

    private void SnapTileToSlot(WordTile argTile)
    {
        argTile.transform.position = this.transform.position;
    }
}
