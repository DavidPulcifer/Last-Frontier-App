using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundArray : MonoBehaviour
{
    public AudioClip[] typingClips;
    public AudioClip[] compSFX;
    public AudioClip startupSFX;
    public AudioClip powerDown;

    public void PlaySingleSound(AudioClip clip, float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }

    public void PlayRandomSound(AudioClip[] clips, float volume)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        volume = Mathf.Clamp(volume, 0, 1);
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }
}
