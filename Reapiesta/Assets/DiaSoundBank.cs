using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaSoundBank : MonoBehaviour
{

    public SfxBank[] banks;

    void Start()
    {
		GetComponent<AudioSource>().pitch = Random.Range(0.7f,1.2f);
        Talk t = GetComponent<Talk>();
		int rng = Random.Range(0, banks.Length);
        for (int i = 0; i < t.lines.Length; i++)
        {
            t.lines[i].clips.Clear();
            t.lines[i].clips.AddRange(banks[rng].lines[i].clips);
        }
        this.enabled = false;
    }
}
