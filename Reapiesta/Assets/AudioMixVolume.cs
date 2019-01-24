using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMixVolume : MonoBehaviour
{

    AudioSource source;
    Slider slider;
    void Start()
    {
        source = GetComponent<AudioSource>();
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        SetVolume(slider.value);
    }

    public void SetVolume(float volume)
    {
		volume /= 2;
		source.outputAudioMixerGroup.audioMixer.SetFloat("Volume" + source.outputAudioMixerGroup.name, Mathf.Log10(volume) * 20);
    }
}
