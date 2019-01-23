using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Talk : MonoBehaviour
{

    public Dia[] lines;
    AudioSource source;
    bool justPlaying = false;
    [HideInInspector] public float curPriority = 0;
    [SerializeField] float silentTime = 2;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.isPlaying == false && justPlaying == true && IsInvoking("DiaDelay") == false)
        {
            Invoke("DiaDelay", silentTime);
        }
    }

    void DiaDelay()
    {
        justPlaying = false;
        curPriority = 0;
    }

    public void Speak(int type, float priority)
    {
        if (source.isPlaying == false)
        {
            if (justPlaying == false)
            {
                source.clip = lines[type].clips[Random.Range(0, lines[type].clips.Count)];
                source.Play();
                justPlaying = true;
                curPriority = priority;
            }
            else if (priority > curPriority)
            {
                CancelInvoke("DiaDelay");
                source.clip = lines[type].clips[Random.Range(0, lines[type].clips.Count)];
                source.Play();
                justPlaying = true;
                curPriority = priority;
            }
        }
        else if (priority > curPriority)
        {
            CancelInvoke("DiaDelay");
            source.clip = lines[type].clips[Random.Range(0, lines[type].clips.Count)];
            source.Play();
            justPlaying = true;
            curPriority = priority;
        }
    }
}

[System.Serializable]
public class Dia
{
    public List<AudioClip> clips;
}
