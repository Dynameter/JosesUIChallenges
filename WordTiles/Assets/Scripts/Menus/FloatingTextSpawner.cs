using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public sealed class FloatingTextSpawner : MonoBehaviour
{
    #region PrivateMembers
    /// <summary>
    /// Path to the floating text prefab.
    /// </summary>
    private const string PATH_TO_FLOATING_TEXT = "Prefabs/WordTiles_FloatingText";

    /// <summary>
    /// The duration that the text should take to move and fade out.
    /// </summary>
    private const float FLOATING_DURATION = 1.5f;

    /// <summary>
    /// The position the text should move up to.
    /// </summary>
    private const float FLOATING_POSITION = 40f;

    /// <summary>
    /// The floating text label prefab.
    /// </summary>
    private static Text _floatingTextPrefab;

    /// <summary>
    /// The floating text label prefab.
    /// </summary>
    private static Text FloatingTextPrefab
    {
        get
        {
            if (_floatingTextPrefab == null)
            {
                _floatingTextPrefab = Resources.Load<Text>(PATH_TO_FLOATING_TEXT);
            }

            return _floatingTextPrefab;
        }
    }

    /// <summary>
    /// Pool used to create labels.
    /// </summary>
    private Pool<Text> m_floatTextPool;
    #endregion PrivateMembers

    #region PrivateMethods
    /// <summary>
    /// Initializes the spawner.
    /// </summary>
    private void Awake()
    {
        m_floatTextPool = new Pool<Text>(CreatePooledFloatingText, null, null, CleanupPooledFloatingText, 1);
    }

    /// <summary>
    /// Coroutine used to position and fade out the floating text.
    /// </summary>
    /// <param name="argFloatingText">The floating text label to move and fade.</param>
    /// <returns>Returns enumerator used to run the coroutine.</returns>
    private IEnumerator DisplayFloatingText(Text argFloatingText)
    {
        Transform textTransform = argFloatingText.transform;

        //Move the label up while fading it out.
        float currentDuration = 0f;
        while (currentDuration < FLOATING_DURATION)
        {
            currentDuration += Time.deltaTime;
            float lerpFactor = (currentDuration / FLOATING_DURATION);

            float currentHeight = Mathf.Lerp(0f, FLOATING_POSITION, lerpFactor);
            textTransform.localPosition = new Vector3(textTransform.localPosition.x, currentHeight, textTransform.localPosition.z);

            float currentAlpha = Mathf.Lerp(1f, 0f, lerpFactor);
            argFloatingText.color = new Color(argFloatingText.color.r, argFloatingText.color.g, argFloatingText.color.b, currentAlpha);

            yield return null;
        }

        //Put the label back in the pool
        m_floatTextPool.Release(argFloatingText);
    }
    #endregion PrivateMethods

    #region PublicMethods
    /// <summary>
    /// Creates a pooled label.
    /// </summary>
    /// <returns>Returns a pooled label.</returns>
    public Text CreatePooledFloatingText()
    {
        Text floatingText = Text.Instantiate<Text>(FloatingTextPrefab);
        floatingText.GetComponent<RectTransform>().SetParent(this.transform, false);

        return floatingText;
    }

    /// <summary>
    /// Hides and cleans up any used floating labels.
    /// </summary>
    /// <param name="argFloatingText">The floating text label to clean up.</param>
    public void CleanupPooledFloatingText(Text argFloatingText)
    {
        argFloatingText.text = string.Empty;
        argFloatingText.enabled = false;
        argFloatingText.transform.position = new Vector3(argFloatingText.transform.position.x, 0f, argFloatingText.transform.position.z);
    }

    /// <summary>
    /// Show the given text in a floating text label.
    /// </summary>
    /// <param name="argText">The text to display.</param>
    public void ShowFloatingText(string argText)
    {
        //Get a pooled label and configure it.
        Text pooledFloatingText = m_floatTextPool.Fetch();
        pooledFloatingText.enabled = true;
        pooledFloatingText.text = argText;
        pooledFloatingText.GetComponent<RectTransform>().SetAsLastSibling();

        StartCoroutine(DisplayFloatingText(pooledFloatingText));
    }
    #endregion PublicMethods
}
