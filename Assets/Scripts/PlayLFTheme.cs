using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLFTheme : MonoBehaviour
{
    [SerializeField] AudioSource mainAudio;
    [SerializeField] AudioClip lcpTheme;

    public void PlayTheme()
    {
        StartCoroutine(PlayThemeThenReturn());
    }

    IEnumerator PlayThemeThenReturn()
    {
        mainAudio.mute = true;
        AudioSource.PlayClipAtPoint(lcpTheme, Camera.main.transform.position);
        yield return new WaitForSeconds(lcpTheme.length);
        mainAudio.mute = false;
    }
}
