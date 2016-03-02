using UnityEngine;
using System.Collections;

public class LagRotation : MonoBehaviour, ILaggable
{
    #region StaticMembers
    /// <summary>
    /// Default rotation speed.
    /// </summary>
    private const float DEFAULT_DEGREES = 1f;

    /// <summary>
    /// Default strength of the spring for the sway lerp.
    /// </summary>
    private const float DEFAULT_SPRING_STRENGTH = 20f;

    /// <summary>
    /// The default number of steps to take for the spring lerp.
    /// </summary>
    private const float DEFAULT_SPRING_STEPS = 1000f;
    #endregion StaticMembers

    #region PrivateMembers
    /// <summary>
    /// The min and max angle the tile can swing back and forth to.
    /// </summary>
    [SerializeField]
    [Tooltip("The min and max angle the tile can swing back and forth to")]
    private float m_swaySpeed = DEFAULT_DEGREES;

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
    #endregion PrivateMembers

    #region PrivateMethods
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
    private void SmoothRotation(float argDeltaTime)
    {
        //Speed can be calculated based off of the current and last position.
        Vector3 deltaPos = (m_transform.position - m_lastPos);
        m_lastPos = m_transform.position;

        //Calculate the angle using a spring lerp
        m_swayAngle += (deltaPos.x * m_swaySpeed);
        m_swayAngle = LerpWithSpring(m_swayAngle, 0f, DEFAULT_SPRING_STRENGTH, Time.deltaTime);
        m_transform.localRotation = Quaternion.Euler(0f, 0f, -m_swayAngle);
    }
    #endregion PrivateMethods

    #region PublicMethods
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
        int steps = Mathf.RoundToInt(argDeltaTime * DEFAULT_SPRING_STEPS);
        argDeltaTime = ((1f / DEFAULT_SPRING_STEPS) * argSpringStrength);

        for (int i = 0; i < steps; ++i)
        {
            argFrom = Mathf.Lerp(argFrom, argTo, argDeltaTime);
        }

        return argFrom;
    }
    #endregion PublicMethods
}
