using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{

    [SerializeField]
    Text txt;
    [SerializeField]
    Image background;
    [SerializeField]
    [TextArea(0, 10)]
    string[] dialogue;
    int curDia = 0;
    [SerializeField]
    Sprite[] backgrounds;
    Color textColor;
    [SerializeField]
    float transitionTime = 3;
    [SerializeField]
    Text nextDiaIndicator;
    AudioSource source;
    [SerializeField]
    AudioClip[] spokenDialogue;
    public int nextScene = 4;

    void Start()
    {
        textColor = txt.color;
        source = GetComponent<AudioSource>();
        source.clip = spokenDialogue[curDia];
        source.Play();
    }

    void LateUpdate()
    {
        NextDia();
        SetText();
        SetBackground();
    }

    void SpokenDialogue()
    {

        source.Stop();
        if (spokenDialogue[curDia] != null)
        {
            source.clip = spokenDialogue[curDia];
            source.Play();

        }


    }

    void NextDia()
    {
        if (Input.GetButtonDown("Attack") == true && background.color == Color.white && nextDiaIndicator.color == textColor)
        {
            if (curDia < dialogue.Length - 1) {
                curDia++;
                SpokenDialogue();
            } else
            {
                Destroy(gameObject);
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    void SetBackground()
    {
        if (background.color != Color.white)
        {
            background.color = Color.Lerp(background.color, Color.white, transitionTime);
        }
        if (backgrounds[curDia] != null)
        {
            background.sprite = backgrounds[curDia];
            if (Input.GetButtonDown("Attack") == true && background.color == Color.white && nextDiaIndicator.color == textColor)
            {
                background.color = Color.clear;
            }
        }
    }

    void SetText()
    {
        txt.text = dialogue[curDia];
        if (Input.GetButtonDown("Attack") == true && background.color == Color.white && nextDiaIndicator.color == textColor)
        {
            txt.color = Color.clear;
        }
        if (txt.color != textColor)
        {
            txt.color = Color.Lerp(txt.color, textColor, transitionTime);
            nextDiaIndicator.color = Color.clear;
        }
        else if (nextDiaIndicator.color != textColor)
        {
            nextDiaIndicator.color = Color.Lerp(nextDiaIndicator.color, textColor, transitionTime);
        }
    }
}
