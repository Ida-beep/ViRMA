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

    private void Start() {
        onHoverColor = new Color32(0,0,0,255);
        hideColor = new Color32(0,0,255,0);
        labelText = gameObject.transform.parent.gameObject.GetComponentInChildren<TextMeshPro>();
        labelText.color = hideColor;
    }

    void OnTriggerEnter(Collider col){
        if(col.GetComponent<ViRMA_Drumstick>())
        {
            //Debug.Log("Sphere colliding with drumsticks!");
            isColliding = true;
            labelText.color = onHoverColor;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<ViRMA_Drumstick>())
        {
            //Debug.Log("Exiting collision");
           isColliding = false;
           labelText.color = hideColor;
        }
    }
}

