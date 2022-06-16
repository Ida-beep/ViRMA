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
    private GameObject questionMarkSphere;
    public GameObject questionMarkPrefab;
    public ViRMA_QuestionMark questionMarkPrefabScript;
    private Collider col;
    public bool showLabel;

    void Awake(){
        questionMarkPrefabScript = questionMarkPrefab.GetComponent<ViRMA_QuestionMark>();
        //Debug.Log(questionMarkPrefab);
    }

    /*New version of SetInline, still testing state: trying to add GO as parent instead of UI element
    + make vector3 the actual position rather than attach position to parent
    + scale offset is introduced*/
    public void SetInline(GameObject componentParent, Vector3 textPositionOffset, Vector3 textScaleOffset, string assignedText, Vector3 textScale){
        // Make Empty Parent
        var newInlineFolder = new GameObject("InlineFolder");
        // Make label and sphere
        //label.MakeLabel(newInlineFolder, textPositionOffset, assignedText,textScale);
        questionMarkSphere = MakeSphere(newInlineFolder, textScaleOffset, textPositionOffset, assignedText, textScale);
        // Set Component as Parent on all elements + offset position
        newInlineFolder.transform.parent = componentParent.transform;
        // Offsets inline according to preference
        newInlineFolder.transform.localPosition = textPositionOffset;
    }

    /*New makessphere for 2D context add localscale and gameobject instead of UI-element*/
    private GameObject MakeSphere(GameObject componentParent,Vector3 scaleOffset, Vector3 textPositionOffset, string assignedText,Vector3 textScale){
        GameObject newInlineSphere = Instantiate(questionMarkPrefab, componentParent.transform.position, Quaternion.identity);
        newInlineSphere.GetComponent<ViRMA_QuestionMark>().SetLabel(componentParent,textPositionOffset,assignedText,textScale);

        newInlineSphere.transform.parent = componentParent.transform;

        newInlineSphere.transform.localPosition = new Vector3(0, 0, 0);
        newInlineSphere.transform.Rotate(0, 90, 0);
        newInlineSphere.transform.localScale = scaleOffset;
        col = newInlineSphere.GetComponent<Collider>();
        return newInlineSphere;
    }



    public void SetInline(ViRMA_UiElement componentParent, Vector3 textPositionOffset, string assignedText,Vector3 textScale){
        /* var newInlineFolder = new GameObject("InlineHelpItem");
        newInlineFolder.transform.parent = componentParent.transform;
        newInlineFolder.transform.localScale = new Vector3(1, 1, 1);
        newInlineFolder.transform.localPosition = textPositionOffset; // moves the entire folder off to one side, should be parametized
        //label.MakeLabel(newInlineFolder, textPositionOffset, assignedText,textScale);
        questionMarkSphere = MakeSphere(newInlineFolder, componentParent, assignedText, textScale); */
    }

    /*Old Makesphere attaching to UI-element*/
    private GameObject MakeSphere(GameObject componentParent, ViRMA_UiElement newUiElement){
        /* GameObject newInlineSphere = Instantiate(questionMarkPrefab, newUiElement.transform.position, Quaternion.identity);
        newInlineSphere.transform.parent = componentParent.transform;
        newInlineSphere.transform.localPosition = new Vector3(0, 0, 0);
        newInlineSphere.transform.Rotate(0, 90, 0);
        col = newInlineSphere.GetComponent<Collider>();
        return newInlineSphere; */
        var go = new GameObject();
        return go;
    }

    /* oid CheckCameraIntersection(){ 
        Ray ray = new Ray(camera.transform.position,camera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if ((Physics.Raycast(ray,out hit,Mathf.Infinity)) && (hit.collider == col)) {
            Debug.Log("SPHERE!!!");
            showLabel = true; 
        } else {
            showLabel = false;
        }
    } */





}
