using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViRMA_Magnifier : MonoBehaviour
{
    public ViRMA_Help help;
    public Transform controller;
    public Canvas canvas;
    float fadeInOutTime = 0.0f;
    [Range(-1.0f,1.0f)]
    public float xPosition = 0.0f;
    [Range(-1.0f,1.0f)]
    public float yPosition = 0.0f;
    [Range(-1.0f,1.0f)]
    public float zPosition = 0.0f;
    [Range(-360.0f,360.0f)]
    public float xrotateBy = 0.0f;//200
    [Range(-360.0f,360.0f)]
    public float yrotateBy = 0.0f;//180
    [Range(-360.0f,360.0f)]
    public float zrotateBy = 0.0f;//180
    public TMPro.TextMeshProUGUI mainText;
    private bool showMagnifier;
    private Dictionary<ViRMA_UiElement,string> uiDic;
    private Dictionary<GameObject,string> goDic;
    private Transform hoverPoint;

    void Awake(){
        uiDic = new Dictionary<ViRMA_UiElement,string>();
        goDic = new Dictionary<GameObject,string>();
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        showMagnifier = false;
    }

    void Start()
    {
        hoverPoint = controller.Find("HoverPoint").GetComponent<Transform>();
        //headline = canvas.Find("Title").GetComponent<TMPro.TextMeshProUGUI>();
        //mainText = canvas.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        if(help.helpIsActive && showMagnifier){
            FadeIn();
            UpdatePosition();
        } else if (!showMagnifier) {
            FadeOut();
        }
    }

    private void UpdatePosition(){
        canvas.transform.position = hoverPoint.position;
        canvas.transform.rotation = hoverPoint.rotation ;
    }

    public void TellMagnifier(GameObject go){
        if(goDic.TryGetValue(go, out string val)){
            this.mainText.text = val;
            showMagnifier = true;
            Debug.Log("value of goDIC = " + val);
        } else {
            Debug.Log("TellMagnifier not working");
        }
    }
    public void TellMagnifier(ViRMA_UiElement ui){
        if(uiDic.TryGetValue(ui, out string val)){
            this.mainText.text = val;
            showMagnifier = true;
        }
    }

    public void AddButton(GameObject go, string mainText){
        goDic.Add(go,mainText);
    }
    public void AddButton(ViRMA_UiElement ui, string mainText){
        //Debug.Log("DIC ADDITION = " + ui);
        uiDic.Add(ui,mainText);
    }

    public void HideMagnifier(){
        showMagnifier = false;
    }


    public void SetMagnifier(string mainText, ViRMA_UiElement btn){
        /* if(btn.)
        this.mainText.text = mainText; */
    }


    void FadeIn(){
        if (canvas != null){
            if(fadeInOutTime < 2){
                fadeInOutTime += Time.deltaTime *8;
                canvas.GetComponent<CanvasGroup>().alpha = fadeInOutTime/1;
                //Debug.Log(canvas.GetComponent<CanvasGroup>().alpha);
            }
        }
    }

    void FadeOut(){
        if (canvas != null){
            if(fadeInOutTime > 0){
                fadeInOutTime -= Time.deltaTime * 1000;
                canvas.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
            }
            fadeInOutTime = 0;
        }
        //canvas.GetComponent<CanvasGroup>().alpha = 0;
    }



}
