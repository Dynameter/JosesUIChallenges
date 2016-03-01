using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class WordTile : SoundButton, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// Function signature for a tile detached callback.
    /// </summary>
    /// <param name="argDetachedTile">The tile that detached.</param>
    public delegate void OnWordTileDetached(WordTile argDetachedTile);

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
    [Tooltip("The label used to display the tile letter")]
    private Text m_letterLabel;

    /// <summary>
    /// Script use to add a "lag" to the tile movement.
    /// </summary>
    [SerializeField]
    [Tooltip("Script used to add a lag to the tile movement")]
    private LagPosition m_lagPosition;

    /// <summary>
    /// Script used to add a swaying effect to the tile movement.
    /// </summary>
    [SerializeField]
    [Tooltip("Script used to add lag to the tile rotation")]
    private LagRotation m_lagRotation;

    /// <summary>
    /// The letter of the tile. A-Z.
    /// </summary>
    private char m_letter;

    /// <summary>
    /// Cached transform.
    /// </summary>
    private RectTransform m_transform;

    /// <summary>
    /// Cached parent.
    /// </summary>
    private RectTransform m_parent;

    /// <summary>
    /// Canvas group that this graphic belongs to. We need to enable and disable it to we can send drop events.
    /// </summary>
    private CanvasGroup m_canvasGroup;

    /// <summary>
    /// Callback called when the tile is detached.
    /// </summary>
    private OnWordTileDetached m_onDetached;

    /// <summary>
    /// Caches all of the components on startup.
    /// </summary>
    protected override void Awake()
    {
        base.Start();

        m_transform = this.GetComponent<RectTransform>();
        m_parent = m_transform.parent.GetComponent<RectTransform>();
        m_canvasGroup = m_parent.GetComponent<CanvasGroup>();

        //If the drag script exists, initialize it.
        if (m_lagPosition != null)
        {
            m_lagPosition.Init();
            m_lagPosition.SetOffset(new Vector3(0f, -(m_transform.rect.height / 1.75f), 0f));
            m_lagPosition.enabled = false;
        }

        //Rotation script should be off
        if (m_lagRotation != null)
        {
            m_lagRotation.Init();
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
        if (IsInteractable() == true)
        {
            //Set the parent to the tray so we can return it if not dropped on a slot.
            WordTileTray tray = WordTilesGameManager.Instance.MainMenuScreen.GetWordTileTray();
            SetGrandParent(tray.transform);

            //Prevent the tile from blocking raycasts so the slot can receive the drop event.
            m_canvasGroup.blocksRaycasts = false;
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

            //Call the detach callback
            if (m_onDetached != null)
            {
                m_onDetached(this);
            }
        }
    }

    /// <summary>
    /// Unity event called every frame while the tile is being dragged.
    /// </summary>
    /// <param name="eventData">The drag event information.</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (IsInteractable() == true)
        {
            m_parent.position = eventData.position;
        }
    }

    /// <summary>
    /// Unity event called when the tile has finished being dragged.
    /// </summary>
    /// <param name="eventData">The drag event information.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsInteractable() == true)
        {
            m_canvasGroup.blocksRaycasts = true;
            m_transform.localScale = REGULAR_SCALE;
            m_transform.rotation = Quaternion.identity;
            m_transform.localPosition = Vector3.one;

            if (m_lagPosition != null)
            {
                m_lagPosition.enabled = false;
            }

            if (m_lagRotation != null)
            {
                m_lagRotation.enabled = false;
            }

            //Check to see if we are attached to a slot. If not, return to the tray.
            WordTileTray tray = WordTilesGameManager.Instance.MainMenuScreen.GetWordTileTray();
            if (m_parent.parent == tray.transform)
            {
                tray.ReturnTile(this);
            }
        }
    }

    /// <summary>
    /// Gets the UI rect transform.
    /// </summary>
    /// <returns>The UI rect transform.</returns>
    public RectTransform GetRectTransform()
    {
        return m_transform;
    }

    /// <summary>
    /// Gets the parent UI rect transform.
    /// </summary>
    /// <returns>Returns the parent UI rect transform.</returns>
    public RectTransform GetParent()
    {
        return m_parent;
    }

    /// <summary>
    /// Sets the grandparent of the tile.
    /// </summary>
    /// <param name="argGrandParent">The grandparent to set.</param>
    /// <param name="argIndex">The sibling index</param>
    public void SetGrandParent(Transform argGrandParent, int argIndex = -1)
    {
        //Attach to the new parent.
        m_parent.SetParent(argGrandParent);

        //Set the right sibling index.
        if (argIndex == -1)
        {
            m_parent.SetAsLastSibling();
        }
        else
        {
            m_parent.SetSiblingIndex(argIndex);
        }

        if (m_onDetached != null)
        {
            m_onDetached(this);
            m_onDetached = null;
        }
    }

    /// <summary>
    /// Sets the callback to call when the tile is detached.
    /// </summary>
    /// <param name="argOnDetach">The callback to call when the tile is detached</param>
    public void SetOnDetachedCallback(OnWordTileDetached argOnDetach)
    {
        m_onDetached = argOnDetach;
    }
}
