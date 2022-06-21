using UnityEngine;
 
public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;

    public Descriptions returnDescriptions(){
        Descriptions arr = JsonUtility.FromJson<Descriptions>(jsonFile.text);
 
        /* foreach (Description description in arr.descriptions)
        {
            Debug.Log("Found : " + description.name + " " + description.text);
        } */

        return arr;
    }
}