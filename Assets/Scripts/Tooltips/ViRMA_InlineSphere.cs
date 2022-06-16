using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class ViRMA_InlineSphere : MonoBehaviour
{
    public bool showLabel = false;
    private Collider col;
    public GameObject inlineSpherePrefab;
    //private GameObject labelRef;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update() {

        /* if(showLabel){
            labelRef.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
        } else if (!showLabel) {
            labelRef.GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 0);
        } */
        //Debug.Log("in Glowsphere, checking isHovering? = " + );
    }

    /*New makessphere for 2D context add localscale and gameobject instead of UI-element*/
    public GameObject MakeSphere(GameObject newGlowFolder, GameObject newUiElement,Vector3 scaleOffset){
        GameObject newInlineSphere = Instantiate(inlineSpherePrefab, newGlowFolder.transform.position, Quaternion.identity);
        newInlineSphere.transform.parent = newGlowFolder.transform;
        newInlineSphere.transform.localPosition = new Vector3(0, 0, 0);
        newInlineSphere.transform.Rotate(0, 90, 0);
        newInlineSphere.transform.localScale = scaleOffset;
        //labelRef = newLabel;
        col = newInlineSphere.GetComponent<Collider>();
        return newInlineSphere;
    }

    public GameObject MakeSphere(GameObject newGlowFolder, ViRMA_UiElement newUiElement){
        GameObject newInlineSphere = Instantiate(inlineSpherePrefab, newUiElement.transform.position, Quaternion.identity);
        newInlineSphere.transform.parent = newGlowFolder.transform;
        newInlineSphere.transform.localPosition = new Vector3(0, 0, 0);
        newInlineSphere.transform.Rotate(0, 90, 0);
        //labelRef = newLabel;
        col = newInlineSphere.GetComponent<Collider>();
        return newInlineSphere;
    }

    void CheckCameraIntersection(){ 
        Ray ray = new Ray(camera.transform.position,camera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if ((Physics.Raycast(ray,out hit,Mathf.Infinity)) && (hit.collider == col)) {
            Debug.Log("SPHERE!!!");
            showLabel = true; 
        } else {
            showLabel = false;
        }
    }

    /* void OnTriggerEnter(Collider col){
     if(col.GetComponent<ViRMA_Drumstick>())
     {
        Debug.Log("Sphere colliding with drumsticks!");
     } */
 }
