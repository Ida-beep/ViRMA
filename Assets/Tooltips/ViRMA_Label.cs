using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class ViRMA_Label : MonoBehaviour
{
    private string _description;
    private Vector3 descriptionPosition;
    private GameObject label = null;
    public GameObject labelPrefab;

    public GameObject Label{
        get {
            if(label==null){
                label = GetComponent<GameObject>();
                return label;
            }
            return null; 
        }
        set {label = value; }
    }

    public GameObject MakeLabel(GameObject newGlowFolder,Vector3 newDescriptionPosition, string newDescription){
        GameObject newLabel = Instantiate(labelPrefab, newDescriptionPosition, Quaternion.identity);
        newLabel.transform.parent = newGlowFolder.transform;
        newLabel.transform.localPosition = new Vector3(40, 0, 0);
        newLabel.transform.localScale = new Vector3(2, 2, 2);
        newLabel.GetComponent<TextMeshPro>().text = newDescription;
        return newLabel;
    }
}