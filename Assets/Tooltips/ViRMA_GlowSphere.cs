using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class ViRMA_GlowSphere : MonoBehaviour
{
    public bool showLabel = false;
    private Collider col;
    public GameObject glowSpherePrefab;
    private GameObject labelRef;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update() {
        CheckCameraIntersection();
        if(showLabel){
            labelRef.GetComponent<TextMeshPro>().color = new Color32(0, 0, 0, 255);
        } else if (!showLabel) {
            labelRef.GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 0);
        }
    }

    public GameObject MakeSphere(GameObject newGlowFolder, ViRMA_UiElement newUiElement, GameObject newLabel){
        GameObject newGlowSphere = Instantiate(glowSpherePrefab, newUiElement.transform.position, Quaternion.identity);
        newGlowSphere.transform.parent = newGlowFolder.transform;
        newGlowSphere.transform.localPosition = new Vector3(0, 0, 0);
        labelRef = newLabel;
        col = newGlowSphere.GetComponent<Collider>();
        return newGlowSphere;
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
