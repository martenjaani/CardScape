using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public AudioSource audioSourcePrefab = null;
    public static AudioSourcePool instance;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private void Awake()
    {
        instance = this;
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying) return source;
        }
        var newSource = Instantiate<AudioSource>(audioSourcePrefab);
        newSource.transform.SetParent(transform,false);
        audioSources.Add(newSource);
        return newSource;
    }

    
}
