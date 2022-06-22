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
    public TMPro.TextMeshProUGUI headline;
    public TMPro.TextMeshProUGUI prevVideo;
    public TMPro.TextMeshProUGUI nextVideo;

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
        welcomeText = gameObject.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        StartPlayingVideo(index);
        animationDescription.color = new Color(0,0,0,0);
        PositionWelcome();

        descriptions = jsonReader.returnDescriptions();
        Debug.Log(descriptions.descriptions[0].text); // GOLDEN CODE
    }

    void LoadAllVideos(){
        for(int i = 0; i < filesAndText.Length/2;i++){
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
        if(index < filesAndText.Length/2){  // since it's a double array it's length/2
            StartPlayingVideo(index);
            UpdateDescription(index);
        } else if (index == filesAndText.Length/2){
            lastVideoPlayed = true;
            DeactivateWelcome();
        }
    }

    public void PlayPreviousVideo(){
        index--;
        if(index >= 0){
            StartPlayingVideo(index);
            UpdateDescription(index);
        }
        if(index < 0){
            index = 0;
        }
    }

    public void UpdateDescription(int index){
        fadeInOutTime = 0.0f;
        animationDescription.text = descriptions.descriptions[index].text;
        animationDescription.color = new Color(0,0,0,0);
        
        headline.text = $"{index}/10 " + descriptions.descriptions[index].name;

        nextVideo.text = descriptions.descriptions[index+1].name;
        prevVideo.text = descriptions.descriptions[index-1].name;

        if(index+1 >= filesAndText.Length/2){
            nextVideo.text = " ";
            //index = filesAndText.Length/2;
        }
        if(index-1 < 0){
            prevVideo.text = " ";
            //index = 0;
        }
    }

    public void StartPlayingVideo(int index){
        test = animationController.GetComponent<UnityEngine.Video.VideoPlayer>();
        test.url = filesAndText[index,0];
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
        welcomeText.position = runtimePosition + new Vector3(0f,13f,-469f); /* static in front */
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

