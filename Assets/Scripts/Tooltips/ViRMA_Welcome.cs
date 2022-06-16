using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_Welcome : MonoBehaviour
{
    private ViRMA_GlobalsAndActions globals;
    public Vector3 position;
    public Camera player;
    private float distanceToPlayer = 10.0f;
    public bool active;
    //private TextMeshPro closeDownText;

    void Awake()
    {
        active = true;
        PositionWelcome();
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        //closeDownText = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        checkMainMenuLoaded();
    }

    void PositionWelcome(){
        Vector3 flattenedVector = Player.instance.bodyDirectionGuess;
        flattenedVector.y = 0;
        flattenedVector.Normalize();
        Vector3 spawnPos = Player.instance.hmdTransform.position + flattenedVector * 0.5f;
        transform.position = new Vector3(spawnPos.x, spawnPos.y-200, (spawnPos.z + distanceToPlayer)*-1);
    }

    public void DeactivateWelcome(/* SteamVR_Action_Boolean action, SteamVR_Input_Sources source */){
        gameObject.transform.position = new Vector3(10000,10000,10000);
    }

    void checkMainMenuLoaded(){
        if(globals.mainMenu.mainMenuLoaded){
            DeactivateWelcome();
        }
    }
}
