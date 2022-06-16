using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViRMa_InlineImplemented : MonoBehaviour
{
    private ViRMA_GlobalsAndActions globals;
    public ViRMA_Help help;

    private void Awake() {
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
    }

    void Update(){

    }
/* 
    void SetupVizInline(){
        if(globals.vizController.vizFullyLoaded){
            Debug.Log("show inline help please");
            help.inline.SetInline(globals.vizController.cellsandAxesWrapper,new Vector3(0,0,0),"Welcome to the VIzController");
        }
    } */
}
