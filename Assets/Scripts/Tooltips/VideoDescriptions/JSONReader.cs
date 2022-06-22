using UnityEngine;
 
public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;

    public Descriptions returnDescriptions(){
        Descriptions arr = JsonUtility.FromJson<Descriptions>(jsonFile.text);
        return arr;
    }
}