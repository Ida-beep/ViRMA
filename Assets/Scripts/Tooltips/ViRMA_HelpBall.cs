using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViRMA_HelpBall : MonoBehaviour
{
    //public GameObject playerBody;

    void Start()
    {
    }

    void Update()
    {
        this.transform.position += transform.forward;
    }
}
