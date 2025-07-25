using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public float oldVolume;

    private void Start()
    {
        oldVolume = slider.value;
        if (PlayerPrefs.HasKey("volume")) slider.value = 0.5f;
        else slider.value = PlayerPrefs.GetFloat("volume");
    }

    private void Update()
    {
        if (oldVolume != slider.value)
        {
            PlayerPrefs.SetFloat("volume", slider.value);
            PlayerPrefs.Save();
            oldVolume = slider.value;
        }
    }
}
