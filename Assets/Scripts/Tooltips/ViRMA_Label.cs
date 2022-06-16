using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class ViRMA_Label : MonoBehaviour
{
/*     private string _description;
    private Vector3 descriptionPosition;
    private GameObject label = null;
    public GameObject labelPrefab;
    private TMPro.TextMeshProUGUI reusableLabel;

    public void Awake(){
        reusableLabel = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Debug.Log(reusableLabel.text);
    }

    void Update(){
    }

    /*MakeLabel using Prefab*/
    /* public void MakeLabel(GameObject newInlineFolder,Vector3 newDescriptionPosition, string newDescription, Vector3 textScale){
        GameObject newLabel = Instantiate(labelPrefab, newDescriptionPosition, Quaternion.identity);
        newLabel.transform.parent = newInlineFolder.transform;
        newLabel.transform.localPosition = new Vector3(0, 0, 0); // 40 0 0 
        newLabel.transform.localScale = textScale;
        newLabel.GetComponent<TextMeshPro>().text = newDescription;
        //return newLabel;
    } */

    /*MakeLabel using reusable TextMeshPro*/
    /* public void FetchLabel(GameObject newInlineFolder,Vector3 newDescriptionPosition, string newDescription, Vector3 textScale){
        reusableLabel.text = newDescription;
        //reusableLabel.transform.parent = newInlineFolder.transform;
        reusableLabel.transform.localPosition = new Vector3(40, 0, 0);
    } */
}