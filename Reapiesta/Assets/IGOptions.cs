using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGOptions : MenuButton
{

    [SerializeField] GameObject newActive;
    [SerializeField] GameObject newUnActive;
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
        newActive.SetActive(true);
        newUnActive.SetActive(false);
    }

    void EventStuff()
    {
        // Application.Quit();
    }
}
