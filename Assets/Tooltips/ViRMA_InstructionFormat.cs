using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_InstructionFormat : MonoBehaviour
{
    public GameObject titleField;
    public GameObject textField;


    void Start()
    {
    }

    void Update()
    {
        // Specify position of title and text
        RectTransform textTransform = textField.GetComponent<RectTransform>();
        textTransform.localRotation =  Quaternion.Euler(180, 180, 0);
    }

    public void SetText(string newTitle, string newInstruction){
        // Specify text-field of each gameobject
        textField.GetComponent<TMPro.TextMeshProUGUI>().text = newInstruction;
        titleField.GetComponent<TMPro.TextMeshProUGUI>().text = newTitle;
        //Debug.Log("title = " + newTitle + " , and instruction is = " + newInstruction);
    }

}
