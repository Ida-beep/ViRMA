using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class ViRMA_Inline : MonoBehaviour
{
    public ViRMA_Help help;
    public ViRMA_Label label;
    public ViRMA_InlineSphere sphere;
    private GameObject testSphere;
    private Collider col;
    public bool showLabel;
    //private GameObject allInlineHelp;

    void Start(){
        //allInlineHelp = new GameObject();
    }

    void Update(){
        /* if(help.helpIsActive){
            allInlineHelp.transform.position += new Vector3(10000,10000,10000);
        } else {
            allInlineHelp.transform.position -= new Vector3(10000,10000,10000);
        } */
    }

    public void SetInline(ViRMA_UiElement newUiElement, Vector3 positionOffset, string newDescription){
        var newInlineFolder = new GameObject("InlineHelpItem");
        //allInlineHelp.transform.parent = newInlineFolder.transform;
        /* Debug.Log(newUiElement.highlighting); */
        newInlineFolder.transform.parent = newUiElement.transform;
        newInlineFolder.transform.localScale = new Vector3(1, 1, 1);
        newInlineFolder.transform.localPosition = positionOffset; // moves the entire folder off to one side, should be parametized
        
        label.MakeLabel(newInlineFolder, positionOffset, newDescription);
        testSphere = sphere.MakeSphere(newInlineFolder,newUiElement /* newLabel */);
    }





}
