using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMixVolume : MonoBehaviour
{

    AudioSource source;
    Slider slider;
    public float generalMultiplier = 0.5f;
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
        volume = Mathf.Max(0.01f,volume);
		volume *= generalMultiplier;
		source.outputAudioMixerGroup.audioMixer.SetFloat("Volume" + source.outputAudioMixerGroup.name, Mathf.Log10(volume) * 20);
    }
}
