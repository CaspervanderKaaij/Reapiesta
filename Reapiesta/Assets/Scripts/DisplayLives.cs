using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayLives : MonoBehaviour
{

    Text txt;
    SaveData data;
    [SerializeField]
    List<Image> kids;

    void Start()
    {
        txt = GetComponent<Text>();
        data = FindObjectOfType<SaveData>();
    }

    void Update()
    {
        //txt.text = "X " + data.lives;
        for (int i = 0; i < kids.Count; i++)
        {
            if (i < data.lives)
            {
                kids[i].enabled = true;
            }
            else
            {
                kids[i].enabled = false;
            }
        }
    }
}
