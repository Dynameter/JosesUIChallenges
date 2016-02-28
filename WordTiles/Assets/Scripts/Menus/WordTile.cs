using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class WordTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// Scale of the tile -before- it is dragged.
    /// </summary>
    private static readonly Vector3 REGULAR_SCALE = Vector3.one;

    /// <summary>
    /// Scale of the tile -while- it is being dragged. Makes it easier to see what you are moving.
    /// </summary>
    private static readonly Vector3 DRAGGED_SCALE = new Vector3(1.25f, 1.25f, 1f);

    /// <summary>
    /// The UI label used to display the letter for this tile.
    /// </summary>
    [SerializeField]
    private Text m_letterLabel;

    [SerializeField]
    private LagPosition m_lagPosition;

    [SerializeField]
    private LagRotation m_lagRotation;

    /// <summary>
    /// The letter of the tile.
    /// </summary>
    private char m_letter;

    /// <summary>
    /// Cached transform.
    /// </summary>
    private RectTransform m_transform;

    /// <summary>
    /// Cached parent.
    /// </summary>
    private Transform m_parent;

    /// <summary>
    /// Canvas group that this graphic belongs to. We need to enable and disable it to we can send drop events.
    /// </summary>
    private CanvasGroup m_canvasGroup;

    /// <summary>
    /// Caches all of the components on startup.
    /// </summary>
    private void Start()
    {
        m_transform = this.GetComponent<RectTransform>();
        m_parent = m_transform.parent;
        m_canvasGroup = m_parent.GetComponent<CanvasGroup>();

        if (m_lagPosition != null)
        {
            m_lagPosition.SetOffset(new Vector3(0f, -(m_transform.rect.height / 1.75f), 0f));
            m_lagPosition.enabled = false;
        }

        if (m_lagRotation != null)
        {
            m_lagRotation.enabled = false;
        }
    }

    /// <summary>
    /// Sets the letter and updates the label.
    /// </summary>
    /// <param name="argLetterToSet">The letter to assign to the tile.</param>
    public void SetLetter(char argLetterToSet)
    {
        m_letter = argLetterToSet;
        m_letterLabel.text = m_letter.ToString();
    }

    /// <summary>
    /// Gets the letter assigned to the tile.
    /// </summary>
    /// <returns>Returns the letter assigned to the tile, A-Z.</returns>
    public char GetLetter()
    {
        return m_letter;
    }

    /// <summary>
    /// Unity event called when the tile is just about to be dragged.
    /// </summary>
    /// <param name="eventData">The drag event information.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_canvasGroup.blocksRaycasts = true;
        m_transform.localScale = DRAGGED_SCALE;

        if (m_lagPosition != null)
        {
            m_lagPosition.Reset();
            m_lagPosition.enabled = true;
        }

        if (m_lagRotation != null)
        {
            m_lagRotation.Reset();
            m_lagRotation.enabled = true;
        }
    }

    /// <summary>
    /// Unity event called every frame while the tile is being dragged.
    /// </summary>
    /// <param name="eventData">The drag event information.</param>
    public void OnDrag(PointerEventData eventData)
    {
        m_parent.position = eventData.position;
    }

    /// <summary>
    /// Unity event called when the tile has finished being dragged.
    /// </summary>
    /// <param name="eventData">The drag event information.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        m_canvasGroup.blocksRaycasts = true;
        m_transform.localScale = REGULAR_SCALE;
        m_transform.rotation = Quaternion.identity;

        if (m_lagPosition != null)
        {
            m_lagPosition.enabled = false;
        }

        if (m_lagRotation != null)
        {
            m_lagRotation.enabled = false;
        }
    }
}
