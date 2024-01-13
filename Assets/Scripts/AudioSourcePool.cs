using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public AudioSource audioSourcePrefab;
    public static AudioSourcePool instance;
    private List<AudioSource> audioSources;
    private void Awake()
    {
        instance = this;
        audioSources = new List<AudioSource>();
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
            if (!source.isPlaying) return source;
        
        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
        audioSources.Add(newSource);
        return newSource;
    }
}
