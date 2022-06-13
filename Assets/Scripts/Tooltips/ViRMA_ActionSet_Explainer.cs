using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_ActionSet_Explainer : MonoBehaviour
{
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
    public ViRMA_Tooltip tooltip;
    public bool showTextBox;


    void Start()
    {
        SetupTextBoxBtn();
        canvas_right.GetComponent<CanvasGroup>().alpha = 0;
        canvas_left.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        CheckIsLookingDown();
        if(playerIsLookingDown){
            ActivateActionSetExplainer();
        } else if (!playerIsLookingDown){
            DeactivateActionSetExplainer();
        }

        /*if(controllerHelpbtnActive){
            //Debug.Log(controllerHelpBtn.transform.position);
        } else {
             //Debug.Log("controller help btn not active!!!!");
        } */

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

    /*
    * Checks if the user is looking down while HELP is active
    */
    void CheckIsLookingDown(){
        if(( Vector3.Dot( headTransform.forward, Vector3.down ) > 0.466f)&& tooltip.helpIsActive){
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
                fadeInOutTime -= Time.deltaTime * 4;
                canvas_right.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
                canvas_left.GetComponent<CanvasGroup>().alpha = fadeInOutTime;
            }
    }
    }

    void SetupTextBoxBtn(){
        controllerHelpBtn.GetComponent<Button>().onClick.AddListener(ToggleTextBox);
    }

    void ToggleTextBox(){
        showTextBox = !showTextBox;
    }

}