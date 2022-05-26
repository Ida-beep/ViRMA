using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViRMA_Glow : MonoBehaviour
{
    /**
    The Glow class is responsible for making a SetGlow() method accesible throughout ViRMA, to make certain UI-elements popout.
    */
    //public GameObject glowFolder;
    public GameObject glowSpherePrefab;
    public GameObject labelPrefab;
    private float time = 0.0f;
    private float delay = 100.0f;
    private bool isFadingIn = true;
    private Collider col;
    private Camera camera;
    public bool showLabel;


    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        CheckCameraIntersection();

    }

    /* void fadeIn()
    {
        while (time <= delay)
        {
            time += Time.deltaTime;
        }
        isFadingIn = false;
    }
    void fadeOut()
    {
        while (time >= 0.0f)
        {
            time -= Time.deltaTime;
        }
        isFadingIn = true;
    } */

    /*
    THIS METHOD WORKS; UNCOMMENT IF THE NEW SETGLOW DOESNT WORK!
    
    SetGlow() take a UI-element to be described, a vector to determin the position of that description, a string and an offset position for the glow-sphere
    */
    /* public void SetGlow(ViRMA_UiElement newUiElement, Vector3 newDescriptionPosition, string newDescription, Vector3 offsetGlowPosition)
    {
        var newGlowFolder = new GameObject("GlowFolder");
        newGlowFolder.transform.parent = newUiElement.transform;
        newGlowFolder.transform.localScale = new Vector3(1, 1, 1);
        newGlowFolder.transform.localPosition = new Vector3(50, 0, 0); // moves the entire folder off to one side, should be parametized

        var newGlow = Instantiate(glowPrefab, newUiElement.transform.position - offsetGlowPosition, Quaternion.identity);
        newGlow.transform.parent = newGlowFolder.transform;
        newGlow.transform.localPosition = new Vector3(0, 0, 0);

        var newLabel = Instantiate(labelPrefab, newDescriptionPosition, Quaternion.identity);
        newLabel.transform.parent = newGlowFolder.transform;
        newLabel.transform.localPosition = new Vector3(40, 0, 0);
        newLabel.transform.localScale = new Vector3(2, 2, 2);
        

        newGlow.GetComponent<Button>().onClick.AddListener(() => TextAppear(newLabel));
    } */

    public void SetGlow(ViRMA_UiElement newUiElement, Vector3 newDescriptionPosition, string newDescription, Canvas labelCanvas){
        var newGlowFolder = new GameObject("GlowItem");
        newGlowFolder.transform.parent = newUiElement.transform;
        newGlowFolder.transform.localScale = new Vector3(1, 1, 1);
        newGlowFolder.transform.localPosition = new Vector3(50, 0, 0); // moves the entire folder off to one side, should be parametized

        var newGlowSphere = Instantiate(glowSpherePrefab, newUiElement.transform.position, Quaternion.identity);
        newGlowSphere.transform.parent = newGlowFolder.transform;
        newGlowSphere.transform.localPosition = new Vector3(0, 0, 0);
        //Debug.Log("in Glow, should I show label?" + newGlowSphere.checkShowLabel());
        
        var newLabel = Instantiate(labelPrefab, newDescriptionPosition, Quaternion.identity);
        newLabel.transform.parent = newGlowFolder.transform;
        newLabel.transform.localPosition = new Vector3(40, 0, 0);
        newLabel.transform.localScale = new Vector3(2, 2, 2);
        newLabel.GetComponent<TMPro.TextMeshProUGUI>().text = "new Text";

        col = newGlowSphere.GetComponent<Collider>();
        Debug.Log("the text of the new label = ");
    }

    void CheckCameraIntersection(){ 
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
    }

}
