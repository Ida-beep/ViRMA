using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

/**
This class is currently under contruction. It's responsible for opening an UI attachment to the right-hand controller
as the user looks at their controller.
TO DO:
- implement delay/timer 
- let the tooltips fade in to create a pleasant experience
- make the class 'context aware' so that it knows what situation the user is in
- design UI to be displayed
- limit the tutorial to looking DOWN!

*/
public class ViRMA_PocketGuide : MonoBehaviour
{
    ViRMA_GlobalsAndActions globals;
    public ViRMA_Help help;
    public ViRMA_PocketGuideFormat format;
    public ViRMA_ActionSet_Explainer actionSetExplainer;
    public GameObject exitTextBoxBtn;
    float fadeInOutTime = 0.0f;
    public Canvas canvas;
    public Transform controller;
    [Range(-360.0f,360.0f)]
    public float xrotateBy = 200f;//200
    [Range(-360.0f,360.0f)]
    public float yrotateBy = 180f;//180
    [Range(-360.0f,360.0f)]
    public float zrotateBy = 180f;//180
    //public Camera camera;
    //public bool showToolTips = false;
    [Range(-1.0f,1.0f)]
    public float xPosition = 0.0f;
    [Range(-1.0f,1.0f)]
    public float yPosition = 0.0f;
    [Range(-1.0f,1.0f)]
    public float zPosition = 0.0f;
    //public bool helpActive = false;
    public UnityEngine.Video.VideoClip mainMenu;
    public UnityEngine.Video.VideoClip keyboard;
    public UnityEngine.Video.VideoClip dimEx;
    public UnityEngine.Video.VideoClip cellContent;
     public UnityEngine.Video.VideoClip browsingState;
     public UnityEngine.Video.VideoClip closeSearch;
     public GameObject video;
     private Vector3 videoLocalPosition;

    void Start()
    {
        videoLocalPosition = video.transform.localPosition;
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        SetupExitBtn();
    }

    void Update()
    {
        if(actionSetExplainer.showPocketGuide && help.helpIsActive){
            ActivateTextBox();
        } else {
            DeactivateTextBox();
        }
    }

    void ActivateTextBox()
    {
            var tagIsProjected = checkBrowsingStateVisible();

            var vp = video.GetComponent<UnityEngine.Video.VideoPlayer>(); 
            fadeIn();
            // Specify position and rotation of the panel
            canvas.transform.position = controller.position + new Vector3(xPosition,yPosition,zPosition);
            canvas.transform.rotation = controller.rotation * Quaternion.Euler(xrotateBy,yrotateBy,zrotateBy);
            // Check state of system
            if(globals.timeline.timelineLoaded){
                format.SetText("Cell Content","Browse images found inside your chosen subcategory");
                format.SetVideo(cellContent,vp);
            } else if (globals.dimExplorer.dimensionExpLorerLoaded && !tagIsProjected){
                format.SetText("Dimension Explorer","These are your search results. Hover and click 'A' to apply as a filter");
                format.SetVideo(dimEx,vp);
            } else if (globals.dimExplorer.dimensionExpLorerLoaded) {
                format.SetText("Closing down search","Click the red button to exit your search and browse the visualisation");
                format.SetVideo(closeSearch,vp);
            } else if (globals.dimExplorer.dimExKeyboard.keyboardLoaded){
                format.SetText("Keyboard","Type in whole or partial words to search. Click the green button to confirm");
                format.SetVideo(keyboard,vp);
            } else if (globals.vizController.vizFullyLoaded){
                format.SetText("Visualisation","Explore the content of subcategories, and how they overlap in the visualisation");
                format.SetVideo(browsingState,vp);
            } else {
                format.SetText("Main Menu","Reach to Hover. Choose between Tags, Time, Location. You can also reposition the menu");
                format.SetVideo(mainMenu,vp);  
            }       
    }

    bool checkBrowsingStateVisible(){
        if(globals.vizController.axisXLine || globals.vizController.axisYLine || globals.vizController.axisZLine){
            return true;
        }
        return false;
    }

    void DeactivateTextBox()
    {
        fadeOut();
    }

    /* void CheckIsLookingDown(){
        //Debug.Log( "checking where the player is looking" ) ;
        if( Vector3.Dot( headTransform.forward, Vector3.down ) > 0.566f)
        {
            //Debug.Log( "I'm looking down!" ) ;
            playerIsLookingDown = true;
        } else {
            //Debug.Log( "I'm NOT!!! looking down!" ) ;
            playerIsLookingDown = false;
        }
    } */

    /* void CheckCameraIntersection()
    { 
        //Debug.Log( "checking for intersection with camera" ) ;
        Ray ray = new Ray(camera.transform.position,camera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,Mathf.Infinity)) {
            showToolTips = true;
        } else {
            showToolTips = false;
        }
    } */

    void fadeIn(){
        if (canvas != null /* && (delayFadeIn <= 0) */){
            if(fadeInOutTime < 2){
                fadeInOutTime += Time.deltaTime;
                canvas.GetComponent<CanvasGroup>().alpha = fadeInOutTime/1;
                video.transform.localPosition = videoLocalPosition;
            }
        }
    }

    void fadeOut(){
        if (canvas != null){
            if(fadeInOutTime > 0){
                fadeInOutTime -= Time.deltaTime * 4;
                canvas.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
                video.transform.localPosition += new Vector3(9999999,9999999,9999999);
            }
        }
    }

    void SetupExitBtn(){
        exitTextBoxBtn.GetComponent<Button>().onClick.AddListener(ExitTextBox);
    }

    void ExitTextBox(){
        actionSetExplainer.showPocketGuide = false;
    }
}
