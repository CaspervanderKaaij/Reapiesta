﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerHitbox : Hitbox
{
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] uint livesAfterDeath = 5;
    PostProcessingBehaviour pp;

    void Start()
    {
        StartStuff();
        pp = Camera.main.GetComponent<PostProcessingBehaviour>();
    }

    public override void Die()
    {
        SaveData save = FindObjectOfType<SaveData>();
        if (save.lives != 0)
        {
            StartCoroutine(DeathEvents());
            StaticFunctions.PlayAudio(2,false,0);
            //Debug.Log(name + " died");
            if (dieShake == true)
            {
                cam.MediumShake();
            }
            if (deathParticle != null)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
            }
        }
        else
        {
            if (dieShake == true)
            {
                cam.MediumShake();
            }
            StartCoroutine(GameOverEvents());
        }

    }

    IEnumerator DeathEvents()
    {
         pp.profile.colorGrading.enabled = true;
        SaveData save = FindObjectOfType<SaveData>();
        Time.timeScale = 0;
        StaticFunctions.paused = true;
        yield return new WaitForSecondsRealtime(1.9f);
        deathUI.SetActive(true);
        StaticFunctions.PlayAudio(13,false,0);
        yield return new WaitForSecondsRealtime(1.3f);
        StaticFunctions.PlayAudio(3,false,0);
        yield return new WaitForSecondsRealtime(0.7f);
        save.lives--;
        SaveLoad.SaveManager(save);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        StaticFunctions.paused = false;
        StaticFunctions.LoadScene(true);
        pp.profile.colorGrading.enabled = false;
    }

    IEnumerator GameOverEvents()
    {
        pp.profile.colorGrading.enabled = true;
        SaveData save = FindObjectOfType<SaveData>();
        Time.timeScale = 0;
        StaticFunctions.paused = true;
        yield return new WaitForSecondsRealtime(1.3f);
        gameOverUI.SetActive(true);
        StaticFunctions.PlayAudio(13,false,0);
        save.lives = livesAfterDeath;
        SaveLoad.SaveManager(save);
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        StaticFunctions.paused = false;
        StaticFunctions.PlayAudio(13,false,0);
        StaticFunctions.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pp.profile.colorGrading.enabled = false;
    }



}
