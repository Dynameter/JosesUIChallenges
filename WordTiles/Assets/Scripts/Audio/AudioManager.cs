using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    #region StaticMembers
    /// <summary>
    /// Singleton instance of the audio manager.
    /// </summary>
    private static AudioManager _instance;

    /// <summary>
    /// Singleton instance of the audio manager.
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Audio_Manager_Singleton_GO").AddComponent<AudioManager>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    #endregion StaticMembers

    #region PrivateMembers
    /// <summary>
    /// Pool that creates reusable audio sources.
    /// </summary>
    private Pool<AudioSource> m_audioSourcePool;

    /// <summary>
    /// List of audio that is playing.
    /// </summary>
    private List<AudioSource> m_playingAudio;

    /// <summary>
    /// Initializes the audio manager.
    /// </summary>
    public void Awake()
    {
        m_audioSourcePool = new Pool<AudioSource>(CreatePooledAudioSource, 2);
        m_playingAudio = new List<AudioSource>();
    }
    #endregion PrivateMembers

    #region PublicMethods
    /// <summary>
    /// Update cleansup any audio sources that are no longer being used.
    /// </summary>
    public void Update()
    {
        for (int i = m_playingAudio.Count - 1; i >= 0; --i)
        {
            if (m_playingAudio[i].isPlaying == false)
            {
                m_audioSourcePool.Release(m_playingAudio[i]);
                m_playingAudio.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Plays an audio clip.
    /// </summary>
    /// <param name="argAudioClip">The audio clip to play.</param>
    public void PlaySound(AudioClip argAudioClip)
    {
        AudioSource source = m_audioSourcePool.Fetch();

        source.clip = argAudioClip;
        source.Play();
        m_playingAudio.Add(source);
    }
    #endregion PublicMethods

    #region PrivateMethods
    /// <summary>
    /// Creates a pooled audio source that will be attached to the manager game object.
    /// </summary>
    /// <returns></returns>
    private AudioSource CreatePooledAudioSource()
    {
        return this.gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Cleans up a poole audio source.
    /// </summary>
    /// <param name="argAudioSource">The pooled audio source to clean up</param>
    private void CleanUpAudioSource(AudioSource argAudioSource)
    {
        argAudioSource.clip = null;
    }
    #endregion PrivateMethods
}
