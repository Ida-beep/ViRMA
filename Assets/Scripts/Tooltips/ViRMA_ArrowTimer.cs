using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViRMA_ArrowTimer : MonoBehaviour
{
    public ViRMA_Help help;
    public Canvas arrow;
    public Canvas arrow_left;
    public Transform right_controller;
    public Transform left_controller;
    [Range(-360.0f,360.0f)]
    public float rotateBy = 200f;
    private float waitTime = 4.0f;
    private float time = 0.0f;
    public bool singleTimerActive = false;
    private bool hasLookedDown = false;

    void Start()
    {
        arrow.transform.position = new Vector3(999999,999999,999999);
        arrow_left.transform.position = new Vector3(999999,999999,999999);
    }

    void Update()
    {
        CheckIsLookingDown();
        CheckWelcomeIsFinished();
        if(singleTimerActive && !hasLookedDown){
            ArrowTimer();
        } else if (hasLookedDown) {
            RemoveArrows();
        }
    }

    void UpdateArrowPosition(){
        arrow.transform.position = right_controller.position;
        arrow.transform.rotation = right_controller.rotation * Quaternion.Euler(rotateBy,180,170);
        arrow_left.transform.position = left_controller.position;
        arrow_left.transform.rotation = left_controller.rotation * Quaternion.Euler(rotateBy,180,170);
    }

    void RemoveArrows(){
        arrow.transform.position = new Vector3(999999,999999,999999);
        arrow_left.transform.position = new Vector3(999999,999999,999999);
    }

    void ArrowTimer(){
        time += Time.deltaTime;
        if(time >= waitTime){
            UpdateArrowPosition();
        }
    }

    void CheckWelcomeIsFinished(){
        if(!help.welcome.active){
            singleTimerActive = true;
        }
    }

    void CheckIsLookingDown(){
        if(help.actionSetExplainer.playerIsLookingDown){
            Debug.Log("NOW HAS LOOKED DOWN");
            hasLookedDown = true;
        }
    }
}
