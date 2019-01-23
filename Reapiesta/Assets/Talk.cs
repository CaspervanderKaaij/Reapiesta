using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Talk : MonoBehaviour
{

    public Dia[] lines;
    AudioSource source;
	bool justPlaying = false;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.isPlaying == false && justPlaying == true && IsInvoking("DiaDelay") == false)
        {
            Invoke("DiaDelay", 2);
        }
    }

    void DiaDelay()
    {
        justPlaying = false;
    }

    public void Speak(int type)
    {
        if (source.isPlaying == false)
        {
            if (justPlaying == false)
            {
                source.clip = lines[type].clips[Random.Range(0, lines[type].clips.Count)];
                source.Play();
				justPlaying = true;
            }
        }
        else
        {
            justPlaying = true;
        }
    }
}

[System.Serializable]
public class Dia
{
    public List<AudioClip> clips;
}
