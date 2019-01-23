using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [HideInInspector] public bool isOver = false;
    [SerializeField] string button = "Attack";
    [SerializeField] Text txt;
    [SerializeField] Image img;
    [SerializeField] bool deselectClick = true;
    [SerializeField] Image backImg;
    [SerializeField] Sprite[] backImages;
    Vector3 startScale;
    void Start()
    {
        StartStuff();
    }

    public void StartStuff()
    {
        if (backImg != null)
        {
            startScale = backImg.rectTransform.localScale;
        }
        if (GetComponent<Text>() != null)
        {
            txt = GetComponent<Text>();
        }
        if (GetComponent<Image>() != null)
        {
            img = GetComponent<Image>();
        }
    }

    void Update()
    {
        UpdateStuff();
    }

    public void UpdateStuff()
    {
        if (isOver == true && Input.GetButtonDown(button) == true)
        {
            ClickEvent();
        }
        if (Input.GetButtonDown(button) == true && deselectClick == true)
        {
            isOver = false;
        }
        else if (isOver == true)
        {
            HighLight();
        }
        else
        {
            DeEmphasize();
        }
    }

    public virtual void ClickEvent()
    {
        StaticFunctions.PlayAudio(0, false);
        StaticFunctions.LoadScene(1);
    }

    public virtual void HighLight()
    {
        if (txt != null)
        {
            // txt.fontStyle = FontStyle.Bold;
            if (backImg != null)
            {
                backImg.sprite = backImages[1];
                backImg.rectTransform.localScale = Vector3.Lerp(backImg.rectTransform.localScale, startScale * 1.1f, Time.deltaTime * 13);
            }
        }
    }

    public virtual void DeEmphasize()
    {
        if (txt != null)
        {
            //  txt.fontStyle = FontStyle.Normal;
            if (backImg != null)
            {
                backImg.sprite = backImages[0];
                backImg.rectTransform.localScale = Vector3.Lerp(backImg.rectTransform.localScale, startScale, Time.deltaTime * 13);
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        isOver = true;
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        isOver = false;
    }
}
