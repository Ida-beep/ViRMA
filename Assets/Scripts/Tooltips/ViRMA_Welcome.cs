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
    private RectTransform welcomeText;

    void Awake()
    {
        active = true;
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        welcomeText = gameObject.GetComponentInChildren<RectTransform>();
        Debug.Log("welcome text = " + welcomeText);
        PositionWelcome();
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
        Vector3 runtimePosition = new Vector3(spawnPos.x, spawnPos.y-200, (spawnPos.z + distanceToPlayer)*-1);
        transform.position = runtimePosition;
        welcomeText.position = runtimePosition + new Vector3(159.8f,54f,-469f);
    }

    public void DeactivateWelcome(){
        gameObject.transform.position = new Vector3(10000,10000,10000);
    }

    void checkMainMenuLoaded(){
        if(globals.mainMenu.mainMenuLoaded){
            DeactivateWelcome();
        }
    }
}
