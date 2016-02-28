using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
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

    private Pool<AudioSource> m_audioSourcePool;
    private List<AudioSource> m_playingAudio;

    public void Awake()
    {
        m_audioSourcePool = new Pool<AudioSource>(CreatePooledAudioSource, 2);
        m_playingAudio = new List<AudioSource>();
    }

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

    private AudioSource CreatePooledAudioSource()
    {
        return this.gameObject.AddComponent<AudioSource>();
    }

    private void CleanUpAudioSource(AudioSource argAudioSource)
    {
        argAudioSource.clip = null;
    }

    public void PlaySound(AudioClip argAudioClip)
    {
        AudioSource source = m_audioSourcePool.Fetch();

        source.clip = argAudioClip;
        source.Play();
        m_playingAudio.Add(source);
    }
}
