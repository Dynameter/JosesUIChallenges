using UnityEngine;
using System.Collections;

public class JellyEffect : MonoBehaviour
{
    public const float DEFAULT_SPEED = 1f;
    public static readonly Vector2 DEFAULT_ITENSITY = new Vector2(0.05f, 0.05f);

    [SerializeField]
    private float m_speed = DEFAULT_SPEED;

    [SerializeField]
    private Vector2 m_intensity = DEFAULT_ITENSITY;

    private Transform m_transform;

    public void Start()
    {
        m_transform = this.transform;
    }

    public void Update()
    {
        m_transform.localScale = new Vector3(1f + Mathf.Abs(Mathf.Cos(Time.time * m_speed)) * m_intensity.x, 1f + Mathf.Abs(Mathf.Sin(Time.time * m_speed)) * m_intensity.y, 1f);
    }
}
