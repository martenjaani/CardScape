using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    public float VolumeMin = 1.0f;
    public float VolumeMax = 1.0f;
    public float PitchMin = 1.0f;
    public float PitchMax = 1.0f;

    public float Cooldown = 0.1f;

    private float nextPlayTime;

    public List<AudioClip> AudioClips;

    private void OnEnable()
    {
        nextPlayTime = 0;
    }

    public void Play(float volumeMultiplier)
    {
        if(AudioSourcePool.instance != null)
            Play(AudioSourcePool.instance.GetSource(), volumeMultiplier);
    }

    public void PlayOnLoop()
    {
        //Play(AudioSourcePool.instance.GetSource());
    }
    public void Play(AudioSource source, float volumeMultiplier)
    {
        if (Time.time < nextPlayTime) return;
        if (AudioClips.Count == 0) return; 
        nextPlayTime = Time.time + Cooldown;
        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        source.volume = Random.Range(VolumeMin, VolumeMax) * volumeMultiplier;
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.Play();
    }

    public void PlayOnLoop(AudioSource source)
    {
        if (Time.time < nextPlayTime) {return; }
        nextPlayTime = Time.time + Cooldown;
        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.loop = true;
        source.Play();
    }
}
