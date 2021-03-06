﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMNewGame : MenuButton
{

    [SerializeField] GameObject loadingObj;
    [SerializeField] Text loadingText;
    [SerializeField] GameObject loadingCircle;
    [SerializeField]
    int sceneToLoad = 0;

    void Start()
    {
        StartStuff();
        StaticFunctions.PlayAudio(13, false,0);
    }

    void Update()
    {
        UpdateStuff();
    }

    public override void ClickEvent()
    {
        Invoke("EventStuff", 1);
        loadingObj.SetActive(true);
    }

    void EventStuff()
    {
        StartCoroutine(SceneLoader());

    }

    IEnumerator SceneLoader()
    {
        //it loads in the background, then when you click, insta load!
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!async.isDone)
        {
            if (Input.GetButtonDown("Attack") == true)
            {
                async.allowSceneActivation = true;
            }
            else
            {
                async.allowSceneActivation = false;
                if (loadingCircle.activeSelf == true)
                {
                    SaveData save = FindObjectOfType<SaveData>();
                    save.lives = 5;
                    save.enemiesLeft = 50;
                    SaveLoad.SaveManager(save);
                    loadingText.text = "Press to continue.";
                    loadingCircle.SetActive(false);
                }
            }
            yield return null;
        }
    }
}
