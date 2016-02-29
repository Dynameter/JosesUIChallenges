using UnityEngine;
using System.Collections;

public class LagRotation : MonoBehaviour, ILaggable
{
    /// <summary>
    /// Default rotation speed.
    /// </summary>
    private const float DEFAULT_DEGREES = 1f;

    [Tooltip("The min and max angle the tile can swing back and forth to")]
    public float swaySpeed = DEFAULT_DEGREES;

    /// <summary>
    /// Cached transform.
    /// </summary>
    private Transform m_transform;

    /// <summary>
    /// Last position of the object.
    /// </summary>
    private Vector3 m_lastPos;

    /// <summary>
    /// Current angle of the object.
    /// </summary>
    private float m_swayAngle = 0f;

    /// <summary>
    /// Caches transform so we can reuse it.
    /// </summary>
    public void Init()
    {
        m_transform = this.transform;
    }

    /// <summary>
    /// Resets the last position and the angle.
    /// </summary>
    public void Reset()
    {
        m_lastPos = m_transform.position;
        m_swayAngle = 0f;
    }

    /// <summary>
    /// Updates the rotation
    /// </summary>
    private void LateUpdate()
    {
        SmoothRotation(Time.deltaTime);
    }

    /// <summary>
    /// Smoothly "sways" the object bases off of movement from last frame.
    /// </summary>
    /// <param name="argDeltaTime">Time passed since the last frame.</param>
    public void SmoothRotation(float argDeltaTime)
    {
        //Speed can be calculated based off of the current and last position.
        Vector3 deltaPos = (m_transform.position - m_lastPos);
        m_lastPos = m_transform.position;

        //Calculate the angle using a spring lerp
        m_swayAngle += (deltaPos.x * swaySpeed);
        m_swayAngle = LerpWithSpring(m_swayAngle, 0f, 20f, Time.deltaTime);
        m_transform.localRotation = Quaternion.Euler(0f, 0f, -m_swayAngle);
    }

    /// <summary>
    /// Lerps to the given value using a spring.
    /// </summary>
    /// <param name="argFrom">The value to lerp from.</param>
    /// <param name="argTo">The value to lerpt to.</param>
    /// <param name="argSpringStrength">The strength of the spring.</param>
    /// <param name="argDeltaTime">The time passed from the last frame.</param>
    /// <returns>Returns the lerped value with a spring applies to it.</returns>
    public static float LerpWithSpring(float argFrom, float argTo, float argSpringStrength, float argDeltaTime)
    {
        argDeltaTime = Mathf.Clamp01(argDeltaTime);
        int steps = Mathf.RoundToInt(argDeltaTime * 1000);
        argDeltaTime = (0.001f * argSpringStrength);

        for (int i = 0; i < steps; ++i)
        {
            argFrom = Mathf.Lerp(argFrom, argTo, argDeltaTime);
        }

        return argFrom;
    }
}
