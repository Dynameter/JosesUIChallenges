using UnityEngine;
using System.Collections;

public class LagPosition : MonoBehaviour, ILaggable
{
    #region StaticMembers
    /// <summary>
    /// Default speed at which the object will catch up to the parent.
    /// </summary>
    private const float DEFAULT_SPEED = 10f;
    #endregion StaticMembers

    #region PrivateMembers
    /// <summary>
    /// Speed at which the object catches up to the parent position
    /// </summary>
    [SerializeField]
    [Tooltip("Speed at which the object catches up to the parent position")]
    private float m_movementSpeed = DEFAULT_SPEED;

    /// <summary>
    /// Cached transform.
    /// </summary>
    private Transform m_transform;

    /// <summary>
    /// Cached parent.
    /// </summary>
    private Transform m_parent;

    /// <summary>
    /// Original position used to calculate movement. Use so adding offset does not cause the object to drift.
    /// </summary>
    private Vector3 m_origPos;

    /// <summary>
    /// The position in local coordinates.
    /// </summary>
    private Vector3 m_relPosition;

    /// <summary>
    /// The position in world coordinates.
    /// </summary>
    private Vector3 m_absPosition;

    /// <summary>
    /// Offset applies to the local position.
    /// </summary>
    private Vector3 m_offset = Vector3.zero;
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Updates the movement.
    /// </summary>
    private void LateUpdate()
    {
        SmoothPosition(Time.deltaTime);
    }

    /// <summary>
    /// Smoothly adds a "lag" between this position and the parent position.
    /// </summary>
    /// <param name="argDelta">Time passed between last frame.</param>
    private void SmoothPosition(float argDelta)
    {
        //Update the position
        Vector3 targPos = (m_parent.position + m_parent.rotation * m_relPosition);
        m_absPosition = Vector3.Lerp(m_absPosition, targPos, Mathf.Clamp01(argDelta * m_movementSpeed));
        m_transform.position = m_absPosition;
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Caches transform values.
    /// </summary>
    public void Init()
    {
        m_transform = this.transform;
        m_parent = m_transform.parent;
        m_origPos = m_transform.localPosition;
    }

    /// <summary>
    /// Resets the positions.
    /// </summary>
    public void Reset()
    {
        m_relPosition = m_origPos + m_offset;
        m_absPosition = m_transform.localToWorldMatrix.MultiplyPoint3x4(m_relPosition);
    }

    /// <summary>
    /// Sets the offset for the drag position.
    /// </summary>
    /// <param name="argOffset"></param>
    public void SetOffset(Vector3 argOffset)
    {
        m_offset = argOffset;
        Reset();
    }
    #endregion PublicMethods
}
