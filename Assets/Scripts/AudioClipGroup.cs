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

    private float nextPlayTime = 0;

    public List<AudioClip> AudioClips;

    public void PLay()
    {
        PLay(AudioSourcePool.instance.GetSource());
    }
    public void PLay(AudioSource source)
    {
        if(Time.time < nextPlayTime) { return; }
        nextPlayTime = Time.time + Cooldown;
        source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.Play();
    }
}
