using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_Help : MonoBehaviour
{
    public ViRMA_ArrowTimer arrowTimer;
    public ViRMA_Magnifier magnifier;
    public ViRMA_Inline inline;
    public ViRMA_UiElement mainHelpBtn;
    public ViRMA_Welcome welcome;
    public ViRMA_ActionSet_Explainer actionSetExplainer;
    public bool helpIsActive; 
    public SteamVR_Action_Vibration vibration;
    private float delay = 100.0f;

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

    void SetupHelpBtn()
    {
        mainHelpBtn.GetComponent<Button>().onClick.AddListener(ToggleHelp);
        //magnifier.AddButton(mainHelpBtn.GetComponent<ViRMA_UiElement>,"click here to toggle help");
        //inline.SetInline(mainHelpBtn,new Vector3(-80,60,0),"Click here to get a little more help!",new Vector3(2,2,2));
    }

    void ToggleHelp(){
        helpIsActive = !helpIsActive;
        mainHelpBtn.Toggle(helpIsActive);
        if (helpIsActive){
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help ON";
            Pulse(0.7f,150,75,SteamVR_Input_Sources.Any);
        } else {
            mainHelpBtn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Help OFF";
        }
    }

    private void Pulse(float duration, float frequency, float amplitute, SteamVR_Input_Sources source){
        vibration.Execute(0,duration,frequency,amplitute,source);
    }


}

