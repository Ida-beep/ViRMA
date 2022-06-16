using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViRMA_QuestionMark : MonoBehaviour
{
    //public ViRMA_Help help;
    public bool isColliding = false;
    //private Color onHoverColor;
    //private Color hideColor;
    //private TextMeshPro labelText;
    private Vector3 positionAtCollision;

    private TMPro.TextMeshProUGUI reusableLabel;
    private GameObject labelController;
    private Canvas reusableCanvas;
    
    private Transform parentComponent;
    public string assignedText;
    private Vector3 textPositionOffset;
    private Vector3 textScale;


    private void Start() {
        labelController = GameObject.Find("LabelController");
        reusableCanvas = labelController.GetComponentInChildren<Canvas>();
        reusableLabel = labelController.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //Debug.Log("at start parent is : " + parentComponent);
        //Debug.Log(reusableLabel.text);
        //Debug.Log(labelController);
        //positionAtCollision = transform.localPosition;
        //onHoverColor = new Color32(0,0,0,255);
        //hideColor = new Color32(0,0,255,0);
        //labelText = gameObject.transform.parent.gameObject.GetComponentInChildren<TextMeshPro>();
        //labelText.color = hideColor;
    }

    void Update(){
        //Debug.Log("the parent is = " + parentComponent);
    }
    public void SetLabel(GameObject newparentComponent, Vector3 textPositionOffset, string assignedText, Vector3 textScale){
        this.parentComponent = newparentComponent.transform;
        this.textPositionOffset = textPositionOffset;
        this.assignedText = assignedText;
        this.textScale = textScale;
        //this.reusableCanvas.transform.localPosition = new Vector3(0,0,0);
        this.positionAtCollision = new Vector3(0,0,0);
        Debug.Log("set label state with parent : " + parentComponent);
    }
    void OnTriggerEnter(Collider col){
        if(col.GetComponent<ViRMA_Drumstick>())
        {
            isColliding = true;
            Debug.Log("cooliding with text : " + assignedText + parentComponent);
            FetchLabel();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<ViRMA_Drumstick>())
        {
           isColliding = false;
           RemoveLabel();
           //labelText.color = hideColor;
        }
    }
    private void FetchLabel(){
        reusableLabel.text = assignedText;
        reusableCanvas.transform.parent = parentComponent;
        reusableCanvas.transform.localScale = textScale;
        //reusableLabel.transform.localPosition = textPositionOffset;
        reusableLabel.transform.localPosition = positionAtCollision;
    }
    private void RemoveLabel(){
        reusableCanvas.transform.localPosition = new Vector3(99999999,99999999,99999999);
    }
}

