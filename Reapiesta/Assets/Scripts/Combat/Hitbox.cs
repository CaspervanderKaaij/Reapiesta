using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitbox : MonoBehaviour
{
    [SerializeField] float curHealth = 3;
    public int team = 0;
    public GameObject deathParticle;
    [SerializeField] GameObject hitParticle;
    [SerializeField] Vector3 particleOffset;
    [Header("ScreenShake")]
    [SerializeField] bool hitShake = false;
    public bool dieShake = false;
    [HideInInspector] public Cam cam;
    [SerializeField] UIPercentBar uiBar;
    float maxHealth = 1;
    [SerializeField] float stopTime = 0.01f;
    [SerializeField] bool playerTalk = false;
    [SerializeField] bool enemyHitSfx = false;
    [SerializeField] float healOverTime = 0;
    public int hitSFX = 21;
    public int deathSFX = 25;

    void Start()
    {
        StartStuff();
    }

    public void StartStuff()
    {
        cam = Camera.main.GetComponent<Cam>();
        maxHealth = curHealth;
    }

    void Update()
    {
        curHealth = Mathf.MoveTowards(curHealth, maxHealth, Time.deltaTime * healOverTime);
        if (uiBar != null)
        {
            uiBar.curPercent = curHealth / maxHealth * 100;
        }
    }
    public virtual void Hit(float damage)
    {
        if (enemyHitSfx == true)
        {
            GetComponent<Talk>().Speak(1, GetComponent<Talk>().curPriority + 1);
        }
        curHealth -= damage;
        //Debug.Log(curHealth + " health left.");
        if (curHealth <= 0)
        {
            Die();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFunctions>().GhostPot(1);
        }
        else if (hitShake == true)
        {
            cam.SmallShake();
            StaticFunctions.PlayAudio(hitSFX, false,0);
            StaticFunctions.PlayAudio(16, false,0);
            //Debug.Log("hi");
            if (StaticFunctions.paused == false)
            {
                Time.timeScale = stopTime;
            }
            StartCoroutine(SetTimeBack(stopTime * 10));
            if (hitParticle != null)
            {
                Instantiate(hitParticle, transform.position + particleOffset, transform.rotation);
            }
        }
        //sets the ui bar if there is one
        if (uiBar != null)
        {
            uiBar.curPercent = curHealth / maxHealth * 100;
        }
    }

    IEnumerator SetTimeBack(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (StaticFunctions.paused == false)
        {
            Time.timeScale = 1f;
        }
        // Debug.Log(StaticFunctions.paused);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        StaticFunctions.PlayAudio(deathSFX, false,0);
        StaticFunctions.PlayAudio(16, false,0);
        if (playerTalk == true)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Talk>().Speak(0, 1);
        }
        //Debug.Log(name + " died");
        if (dieShake == true)
        {
            cam.MediumShake();
        }
        if (deathParticle != null)
        {
            Instantiate(deathParticle, transform.position + particleOffset, transform.rotation);
        }
    }
}
