using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MenuButton
{

[SerializeField] GameObject loadObj;
    void Start()
    {
        StartStuff();
    }

    void Update()
    {
        UpdateStuff();
    }

    public override void ClickEvent()
    {
        //  Invoke("EventStuff", 1);
		loadObj.SetActive(true);
		Time.timeScale = 1;
		StaticFunctions.paused = false;
        StaticFunctions.LoadScene(0);
    }

    void EventStuff()
    {
        //StaticFunctions.LoadScene(0);
    }
}
