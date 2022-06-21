using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;
using System.IO;

public class ViRMA_Welcome : MonoBehaviour
{
    private ViRMA_Help help;
    private ViRMA_GlobalsAndActions globals;
    public Vector3 position;
    public Camera player;
    private float distanceToPlayer = 10.0f;
    public bool active;
    private RectTransform welcomeText;
    public GameObject animationController;
    private UnityEngine.Video.VideoPlayer test;
    private string [,] filesAndText;
    private string [] textDescriptions;

    private int index;
    public Canvas arrow;
    public Transform right_controller;
    public bool lastVideoPlayed;
    public TMPro.TextMeshProUGUI animationDescription;

    private float fadeInOutTime = 0.0f;

    public JSONReader jsonReader;
    private Descriptions descriptions;

    void Awake()
    {
        index = 0;
        lastVideoPlayed = false;

        filesAndText = new string [11,2];
        LoadAllVideos();

        active = true;
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        help = globals.help;
        welcomeText = gameObject.GetComponentInChildren</* RectTransform */Canvas>().GetComponent<RectTransform>();
        StartPlayingVideo(index);
        animationDescription.color = new Color(0,0,0,0);
        PositionWelcome();

        descriptions = jsonReader.returnDescriptions();
        Debug.Log(descriptions.descriptions[0].text); // GOLDEN CODE
    }

    void LoadAllVideos(){
        for(int i = 0; i < filesAndText.Length/2;i++){
            //Debug.Log("index is ? = " + i );
            filesAndText[i,0] = $"Assets/Resources/Video/Clip_{i}.mp4";
            filesAndText[i,1] = $"{i}/10    some description";
        }
    }

    void Update()
    {
        fadeIn();
        checkMainMenuLoaded();
        UpdateArrowPosition();
    }

    void UpdateArrowPosition(){
        if(active){
            arrow.transform.position = right_controller.position;
        } else {
            arrow.transform.position = new Vector3(999999,999999,999999);
        }
    }


    public void PlayNextVideo(){
        index++;
        Debug.Log("length of array = " + filesAndText.Length);
        Debug.Log("NEXXXT: index is : " + index );
        if(index < filesAndText.Length/2){  // since it's a double array it's length/2
            StartPlayingVideo(index);
            UpdateDescription(index);
        } else if (index == filesAndText.Length/2){
            Debug.Log("DEACTIVATE");
            lastVideoPlayed = true;
            DeactivateWelcome();
        }
    }

    public void PlayPreviousVideo(){
        index++;
        Debug.Log("index is : " + index);
        if(index > 0){
            StartPlayingVideo(index);
            UpdateDescription(index);
        }
    }

    public void UpdateDescription(int index){
        Debug.Log("index is : " + index);
        fadeInOutTime = 0.0f;
        animationDescription.text = filesAndText[index,1];
        animationDescription.color = new Color(0,0,0,0);
        Debug.Log("text is = " + animationDescription);
    }

    public void StartPlayingVideo(int index){
        //Debug.Log("index is : " + index);
        test = animationController.GetComponent<UnityEngine.Video.VideoPlayer>();
        Debug.Log("inside playing meethod index is = " + index);
        test.url = filesAndText[index,0];
        //Debug.Log("inside playing meethod index is = " + index);
        test.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        test.targetMaterialRenderer = GetComponent<Renderer>();
        //test.targetMaterialPro perty = "_MainTex";
        if (index == 0){
            test.isLooping = false;
            test.playOnAwake = true;            
        } else {
            test.isLooping = true;
            test.playOnAwake = true;  
        }
        test.Play();
    }

    void PositionWelcome(){
        Vector3 flattenedVector = Player.instance.bodyDirectionGuess;
        flattenedVector.y = 0;
        flattenedVector.Normalize();
        Vector3 spawnPos = Player.instance.hmdTransform.position + flattenedVector * 0.5f;
        Vector3 runtimePosition = new Vector3(spawnPos.x, spawnPos.y-200, (spawnPos.z + distanceToPlayer)*-1);
        transform.position = runtimePosition;
        welcomeText.position = runtimePosition + new Vector3(159.8f,54f,-469f); /* static in front */
        //welcomeText.position = runtimePosition + new Vector3(spawnPos.x,spawnPos.y,spawnPos.z);
    }

    public void DeactivateWelcome(){
        gameObject.transform.position = new Vector3(10000,10000,10000);
        active = false;
    }

    void checkMainMenuLoaded(){
        if(globals.mainMenu.mainMenuLoaded){
            DeactivateWelcome();
        }
    }

    void fadeIn(){
        if(fadeInOutTime < 2){
            fadeInOutTime += Time.deltaTime;
            animationDescription.color = new Color(0,0,0,fadeInOutTime/1);
            animationDescription.color = new Color(0,0,0,fadeInOutTime/1);
        }

    }
}

[System.Serializable]
public class ListDescriptions{
    public List<Description> descriptions;
}

