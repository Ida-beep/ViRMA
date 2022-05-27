using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collision col){
    if(col.GetComponent<ViRMA_Drumstick>())
    {
        Debug.Log("Sphere colliding with drumsticks!");
    }
}
}

