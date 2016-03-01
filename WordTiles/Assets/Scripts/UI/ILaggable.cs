using UnityEngine;
using System.Collections;

/// <summary>
/// Interface for any script that applies lag to position, rotation, etc.
/// </summary>
public interface ILaggable
{
    /// <summary>
    /// Initializes the laggable script.
    /// </summary>
    void Init();

    /// <summary>
    /// Resets the laggable script
    /// </summary>
    void Reset();
}
