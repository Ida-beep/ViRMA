using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViRMA_GlowSphere : MonoBehaviour
{

    /* private ViRMA_UiElement uiElement;
    private Vector3 descriptionPosition;
    private string description; */
    //public GameObject labelPrefab;
    //public GameObject glowPrefab;
    private Camera camera;
    public bool showLabel = false;
    private Collider col;

    /* public ViRMA_GlowItem(ViRMA_UiElement newUiElement, Vector3 newDescriptionPosition, string newDescription, GameObject glowFolder){
        var newGlowFolder = new GameObject("GlowFolder");
        newGlowFolder.transform.parent = newUiElement.transform;
        newGlowFolder.transform.localScale = new Vector3(1, 1, 1);
        newGlowFolder.transform.localPosition = new Vector3(50, 0, 0);
        
        var newLabel = Instantiate(labelPrefab, newDescriptionPosition, Quaternion.identity);
        //newLabel.transform.parent = newGlowFolder.transform;
        newLabel.transform.localPosition = new Vector3(40, 0, 0);
        newLabel.transform.localScale = new Vector3(2, 2, 2);

        var newGlow = Instantiate(glowPrefab, newUiElement.transform.position, Quaternion.identity);
        //newGlow.transform.parent = newGlowFolder.transform;
        newGlow.transform.localPosition = new Vector3(0, 0, 0);

        newLabel.transform.parent = glowFolder.transform;
        newGlow.transform.parent = glowFolder.transform;
        
        uiElement = newUiElement;
        description = newDescription;
        descriptionPosition = newDescriptionPosition;
    } */

    void Start()
    {
        camera = Camera.main;
        col = gameObject.GetComponent<Collider>();
        //Debug.Log(col);
    }

    void Update()
    {
        //CheckCameraIntersection();
    }

    /* public void SetGlowItem(ViRMA_UiElement newUiElement, Vector3 newDescriptionPosition, string newDescription){
        uiElement = newUiElement;
        description = newDescription;
        descriptionPosition = newDescriptionPosition;
    } */

    /* public ViRMA_UiElement GetUiElement(){
        return uiElement;
    }
    public Vector3 GetDescriptionPosition(){
        return descriptionPosition;
    }
    public string GetDescription(){
        return description;
    } */

    /* void CheckCameraIntersection()
    { 
        Ray ray = new Ray(camera.transform.position,camera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,Mathf.Infinity)) {
            if(hit.collider == col){
                Debug.Log("SPHERE!!!");
                showLabel = true;
            }
        } else {
            showLabel = false;
        }
    } */

    public bool checkShowLabel(){
        return showLabel;
    }

    /* void SetupoClick(){
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick(){
        showLabel =! showLabel;
        Debug.Log("did click!");
    } */

}