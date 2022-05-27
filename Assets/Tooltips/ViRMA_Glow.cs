using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class ViRMA_Glow : MonoBehaviour
{

    // Prefabs
    public GameObject glowSpherePrefab;

    // Label
    public ViRMA_Label label;
    public ViRMA_GlowSphere sphere;

    private Collider col;
    //private Camera camera;
    public bool showLabel;

    void Start()
    {
        //camera = Camera.main;
    }

    void Update()
    {
        //CheckCameraIntersection();

    }


    public void SetGlow(ViRMA_UiElement newUiElement, Vector3 newDescriptionPosition, string newDescription, Canvas labelCanvas){
        var newGlowFolder = new GameObject("GlowItem");
        newGlowFolder.transform.parent = newUiElement.transform;
        newGlowFolder.transform.localScale = new Vector3(1, 1, 1);
        newGlowFolder.transform.localPosition = new Vector3(50, 0, 0); // moves the entire folder off to one side, should be parametized
        
        GameObject newLabel = label.MakeLabel(newGlowFolder, newDescriptionPosition, newDescription);
        GameObject newSphere = sphere.MakeSphere(newGlowFolder,newUiElement, newLabel);

        //col = sphere.GetComponent<Collider>();
    }





}
