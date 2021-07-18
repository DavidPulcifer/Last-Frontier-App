using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCarousel : MonoBehaviour
{
    [SerializeField] Slider adTimeSlider;
    [SerializeField] Image adImage;
    [SerializeField] Sprite[] adArray;

    int currentAdIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentAdIndex = Random.Range(0, adArray.Length);

        adImage.sprite = adArray[currentAdIndex];
        adTimeSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        adTimeSlider.value = Mathf.Clamp(adTimeSlider.value + Time.deltaTime, 0f, 5f);
        if(Mathf.Approximately(adTimeSlider.maxValue, adTimeSlider.value))
        {
            UpdateAd();
        }
    }

    void UpdateAd()
    {
        if(currentAdIndex >= adArray.Length-1)
        {
            currentAdIndex = 0;
        }
        else
        {
            currentAdIndex++;
        }

        adImage.sprite = adArray[currentAdIndex];
        adTimeSlider.value = 0;
    }
}
