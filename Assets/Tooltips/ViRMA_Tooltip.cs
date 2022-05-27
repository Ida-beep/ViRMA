using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_Tooltip : MonoBehaviour
{
    public ViRMA_Glow glow;
    public Canvas labelCanvas;
    public GameObject labelPrefab;
    public ViRMA_UiElement mainHelpBtn;
    public bool helpIsActive;

    void Start()
    {
        SetupHelpBtn();

    }

    // TEXT BOX : Creating Panels with text attached to controller 
    void CreateToolTip(string title, string text){
        Debug.Log("create controller panel");
    }

    // TEXT BOX : Text box triggered by hover on UI-element 
    void CreateToolTip(string title, string text, ViRMA_UiElement uielement){
        Debug.Log("create controller panel");
    }

    // LABEL : Adding label in 3D space 
    void CreateToolTip(string label, Vector3 placement){
        GameObject l = Instantiate(labelPrefab, placement, Quaternion.identity) as GameObject; // Please check rotation in debugging later
        l.transform.SetParent(labelCanvas.transform, false); // check if it's working
    }

    // ACTIONSET EXPLANATION : Controller mappings, depending on certain actionset
    void CreateToolTip(string actionSetName, string explanation, string button){
        Debug.Log("create controller mapping explanation");
    }

    void SetupHelpBtn()
    {
        mainHelpBtn.GetComponent<Button>().onClick.AddListener(ToggleHelp);
        var helpbtnCanvas = mainHelpBtn.GetComponentInParent<Canvas>();
        glow.SetGlow(mainHelpBtn,new Vector3(0,0,0),"Click here to get a little more help!",helpbtnCanvas);

    }

    void ToggleHelp(){
        helpIsActive = !helpIsActive;
    }

}

