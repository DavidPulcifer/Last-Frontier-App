using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllegianceSlider : MonoBehaviour
{
    [SerializeField] Slider allegianceSlider;
    [SerializeField] Image resistanceImage;
    [SerializeField] Image ICPImage;
    [SerializeField] [Range(0, 1)] float fadedAlpha = 0.25f;
    [SerializeField] [Range(0, 1)] float halfAlpha = 0.5f;
    [SerializeField] [Range(0, 1)] float fullAlpha = 1f;
    [SerializeField] TextMeshProUGUI descriptionText;

    string resistanceDescription = "You support colonial independence.";
    string undecidedDescription = "You support neither side.";
    string ICPDescription = "You support the ICP.";

    int sliderChoice = 1;

    const string AllegianceSliderKey = "ALLEGIANCE SLIDER KEY";


    private void Awake()
    {
        allegianceSlider.value = PlayerPrefs.GetInt(AllegianceSliderKey);
    }

    private void Start()
    {
        updateLogos();
    }

    void Update()
    {
        if(sliderChoice != (int)allegianceSlider.value)
        {
            updateLogos();
        }
    }

    void updateLogos()
    {
        sliderChoice = (int)allegianceSlider.value;
        PlayerPrefs.SetInt(AllegianceSliderKey, sliderChoice);
        Color resistanceColor = resistanceImage.color;
        Color ICPColor = ICPImage.color;

        switch (sliderChoice)
        {            
            case 0:                
                resistanceColor.a = fullAlpha;
                ICPColor.a = fadedAlpha;
                descriptionText.text = resistanceDescription;
                break;
            case 1:
                resistanceColor.a = halfAlpha;
                ICPColor.a = halfAlpha;
                descriptionText.text = undecidedDescription;
                break;
            case 2:
                resistanceColor.a = fadedAlpha;
                ICPColor.a = fullAlpha;
                descriptionText.text = ICPDescription;
                break;
            default:
                resistanceColor.a = fullAlpha;
                ICPColor.a = fullAlpha;
                descriptionText.text = "";
                break;
        }

        resistanceImage.color = resistanceColor;
        ICPImage.color = ICPColor;
    }
}
