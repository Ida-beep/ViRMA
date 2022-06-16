using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_Help : MonoBehaviour
{
    public ViRMA_Inline inline;
    public ViRMA_UiElement mainHelpBtn;
    public ViRMA_Welcome welcome;
    public ViRMA_ActionSet_Explainer actionSetExplainer;
    public bool helpIsActive; 
    public SteamVR_Action_Vibration vibration;

    void Start()
    {
        helpIsActive = true;
        if (helpIsActive){
            mainHelpBtn.Toggle(helpIsActive);
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help ON";
        } else {
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help OFF";
        }

        SetupHelpBtn();
    }

    // TEXT BOX : Creating Panels with text attached to controller 
    /* void CreateToolTip(string title, string text){
        Debug.Log("create controller panel");
    } */

    // TEXT BOX : Text box triggered by hover on UI-element 
    /* void CreateToolTip(string title, string text, ViRMA_UiElement uielement){
        Debug.Log("create controller panel");
    } */

    // LABEL : Adding label in 3D space 
    /* void CreateToolTip(string label, Vector3 placement){
        GameObject l = Instantiate(labelPrefab, placement, Quaternion.identity) as GameObject; // Please check rotation in debugging later
        l.transform.SetParent(labelCanvas.transform, false); // check if it's working
    } */

    // ACTIONSET EXPLANATION : Controller mappings, depending on certain actionset
    /* void CreateToolTip(string actionSetName, string explanation, string button){
        Debug.Log("create controller mapping explanation");
    } */

    void SetupHelpBtn()
    {
        mainHelpBtn.GetComponent<Button>().onClick.AddListener(ToggleHelp);
        //var helpbtnCanvas = mainHelpBtn.GetComponentInParent<Canvas>();
        inline.SetInline(mainHelpBtn,new Vector3(-80,60,0),"Click here to get a little more help!",new Vector3(2,2,2));
    }

    void ToggleHelp(){
        helpIsActive = !helpIsActive;
        mainHelpBtn.Toggle(helpIsActive);
        if (helpIsActive){
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help ON";
            Pulse(0.25f,150,75,SteamVR_Input_Sources.Any);
        } else {
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help OFF";
        }
    }

    private void Pulse(float duration, float frequency, float amplitute, SteamVR_Input_Sources source){
        vibration.Execute(0,duration,frequency,amplitute,source);
    }


}

