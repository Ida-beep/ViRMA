using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collision : MonoBehaviour
{
    public ViRMA_Help help;
    public bool isColliding = false;
    private Color onHoverColor;
    private Color hideColor;
    private TextMeshPro labelText;
    private Vector3 position;

    private void Start() {
        position = transform.localPosition;
        //Debug.Log("position of an element is: " + position);
        onHoverColor = new Color32(0,0,0,255);
        hideColor = new Color32(0,0,255,0);
        labelText = gameObject.transform.parent.gameObject.GetComponentInChildren<TextMeshPro>();
        labelText.color = hideColor;
    }

    void Update(){
        /* if(help.helpIsActive){
            transform.localPosition = position;
        } else {
            transform.localPosition = new Vector3(999999,999999,999999);
        } */
    }

    void OnTriggerEnter(Collider col){
        if(col.GetComponent<ViRMA_Drumstick>())
        {
            isColliding = true;
            labelText.color = onHoverColor;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<ViRMA_Drumstick>())
        {
           isColliding = false;
           labelText.color = hideColor;
        }
    }
}

