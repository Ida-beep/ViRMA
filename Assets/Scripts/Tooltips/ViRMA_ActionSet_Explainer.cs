using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_ActionSet_Explainer : MonoBehaviour
{
    private ViRMA_GlobalsAndActions globals;
    // Looking down-feature
    bool playerIsLookingDown = false;
    public Transform headTransform;
    public Transform right_controller;
    public Transform left_controller;
    // TEST Canvas right and left
    public Canvas canvas_left;
    public Canvas canvas_right;
    [Range(-360.0f,360.0f)]
    public float rotateBy = 200f;
    float fadeInOutTime = 0.0f;
    public GameObject controllerHelpBtn;
    //public bool controllerHelpbtnActive = false;
    public ViRMA_Help help;
    public bool showPocketGuide;

    public GameObject triggerLeftLine;
    public GameObject triggerRightLine;
    public GameObject triggerLeft;
    public GameObject triggerRight;
    public GameObject BLeft;
    public GameObject Bright;
    public GameObject ALeft;
    public GameObject ARight;

    void Start()
    {
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        //SetupTextBoxBtn();
        canvas_right.GetComponent<CanvasGroup>().alpha = 0;
        canvas_left.GetComponent<CanvasGroup>().alpha = 0;
        SetDefaultActionDetails();
    }

    void Update()
    {
        CheckIsLookingDown();
        if(playerIsLookingDown && !showPocketGuide){
            ActivateActionSetExplainer();
            Debug.Log("Shoudl ACTIVATE");
        } else if (!playerIsLookingDown || showPocketGuide){
            DeactivateActionSetExplainer();
            Debug.Log("Shoudl deactivate");
        }
        SetDynamicActionDetails();
    }

    void ActivateActionSetExplainer(){
        fadeIn();
        canvas_right.transform.position = right_controller.position;
        canvas_right.transform.rotation = right_controller.rotation * Quaternion.Euler(rotateBy,180,190);
        canvas_left.transform.position = left_controller.position;
        canvas_left.transform.rotation = left_controller.rotation * Quaternion.Euler(rotateBy,180,170);
    }

    void DeactivateActionSetExplainer()
    {
        fadeOut();
    }

    void CheckIsLookingDown(){
        if(( Vector3.Dot( headTransform.forward, Vector3.down ) > 0.466f)&& help.helpIsActive){
            playerIsLookingDown = true;
        } else {
            playerIsLookingDown = false;
        }
    }

    void fadeIn(){
        if (canvas_right != null && canvas_left != null){
            if(fadeInOutTime < 2){
                fadeInOutTime += Time.deltaTime;
                canvas_right.GetComponent<CanvasGroup>().alpha = fadeInOutTime/1;
                canvas_left.GetComponent<CanvasGroup>().alpha = fadeInOutTime/1;
            }
        }
    }

    void fadeOut(){
        if (canvas_right != null && canvas_left != null){
            if(fadeInOutTime > 0){
                fadeInOutTime -= Time.deltaTime * 8;
                canvas_right.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
                canvas_left.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
            }
        }
    }

    /* void SetupTextBoxBtn(){
        controllerHelpBtn.GetComponent<Button>().onClick.AddListener(ToggleTextBox);
    } */

    /* void ToggleTextBox(){
        showPocketGuide = !showPocketGuide;
    } */

    public void TogglePocketGuide(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
       showPocketGuide = !showPocketGuide;
    }




    void SetDefaultActionDetails(){
        ALeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Select";
        ARight.GetComponent<TMPro.TextMeshProUGUI>().text = "Select";
        BLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Main Menu";
        Bright.GetComponent<TMPro.TextMeshProUGUI>().text = "Main Menu";
        triggerLeft.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
        triggerRight.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
        triggerLeftLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0,0);
        triggerRightLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0,0);
    }

    void SetDynamicActionDetails(){
        if (!globals.dimExplorer.dimExKeyboard.keyboardLoaded)
        {
            triggerLeft.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
            triggerRight.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
            triggerLeftLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0,0);
            triggerRightLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0,0);
        }
        if (globals.dimExplorer.dimensionExpLorerLoaded)
        {
            triggerLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Drag";
            triggerRight.GetComponent<TMPro.TextMeshProUGUI>().text = "Drag";
            triggerLeftLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0);
            triggerRightLine.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0,0,0);
        }
        else if (globals.vizController.vizFullyLoaded)
        {
            triggerLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Rotate";
            BLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Main Menu";
            Bright.GetComponent<TMPro.TextMeshProUGUI>().text = "Main Menu";
        }
        if (globals.timeline.timelineLoaded)
        {
            BLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Back";
            Bright.GetComponent<TMPro.TextMeshProUGUI>().text = "Back";
            triggerLeft.GetComponent<TMPro.TextMeshProUGUI>().text = "Drag";
        }
    }

}