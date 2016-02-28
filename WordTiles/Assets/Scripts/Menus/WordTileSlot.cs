using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class WordTileSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
        }
        else
        {
            Debug.Log("Nothing");
        }
    }
}
