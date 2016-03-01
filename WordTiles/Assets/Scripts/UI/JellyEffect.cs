using UnityEngine;
using System.Collections;

public class JellyEffect : MonoBehaviour
{
    /// <summary>
    /// Default speed of the jelly effect.
    /// </summary>
    public const float DEFAULT_SPEED = 1f;

    /// <summary>
    /// Default intensity of the jelly effect. Tied to how much the object should scale up by.
    /// </summary>
    public static readonly Vector2 DEFAULT_ITENSITY = new Vector2(0.05f, 0.05f);

    /// <summary>
    /// The speed of the jelly effect
    /// </summary>
    [SerializeField]
    [Tooltip("Default speed of the jelly effect.")]
    private float m_speed = DEFAULT_SPEED;

    /// <summary>
    /// The intensity of the jelly effect.
    /// </summary>
    [SerializeField]
    [Tooltip("Default intensity of the jelly effect. Tied to how much the object should scale up by.")]
    private Vector2 m_intensity = DEFAULT_ITENSITY;

    /// <summary>
    /// Cached transform.
    /// </summary>
    private Transform m_transform;

    /// <summary>
    /// Initializes the jelly effect.
    /// </summary>
    public void Start()
    {
        m_transform = this.transform;
    }

    /// <summary>
    /// Update applies the jelly effect.
    /// </summary>
    public void Update()
    {
        m_transform.localScale = new Vector3(1f + Mathf.Abs(Mathf.Cos(Time.time * m_speed)) * m_intensity.x, 1f + Mathf.Abs(Mathf.Sin(Time.time * m_speed)) * m_intensity.y, 1f);
    }
}
