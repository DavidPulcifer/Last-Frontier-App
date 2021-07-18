using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHackingPuzzle : MonoBehaviour
{
    [SerializeField] Canvas hackingCanvas;
    [SerializeField] Canvas scanningCanvas;
    [SerializeField] Slider scanningSlider;
    [SerializeField] SoundArray soundPlayer;
    [SerializeField] float scanningTime = 5f;
    [SerializeField] float initialAppearDelay = 0.5f;
    [SerializeField] float appearDelay = 0.1f;

    bool scanningComplete = false;

    private void Start()
    {
        scanningSlider.value = 0;
        scanningSlider.maxValue = scanningTime;
        StartCoroutine(AppearScanningCanvas());
    }

    private void Update()
    {
        if (scanningComplete) return;

        scanningSlider.value = Mathf.Min(scanningSlider.value + Time.deltaTime, scanningSlider.maxValue);

        if (Mathf.Approximately(scanningSlider.value, scanningSlider.maxValue))
        {
            scanningComplete = true;
            scanningCanvas.gameObject.SetActive(false);
            hackingCanvas.gameObject.SetActive(true);
            soundPlayer.PlayRandomSound(soundPlayer.typingClips, 0.5f);
        }
    }

    IEnumerator AppearScanningCanvas()
    {
        scanningCanvas.gameObject.SetActive(true);
        soundPlayer.PlaySingleSound(soundPlayer.startupSFX, 0.5f);
        yield return new WaitForSeconds(initialAppearDelay);
        foreach (Transform child in scanningCanvas.gameObject.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(appearDelay);
        }
        yield return new WaitForSeconds(scanningSlider.maxValue / 6);
        soundPlayer.PlayRandomSound(soundPlayer.compSFX, 0.5f);
        yield return new WaitForSeconds(scanningSlider.maxValue / 6);
        soundPlayer.PlayRandomSound(soundPlayer.compSFX, 0.5f);
        yield return new WaitForSeconds(scanningSlider.maxValue / 6);
        soundPlayer.PlayRandomSound(soundPlayer.compSFX, 0.5f);
    }
}
