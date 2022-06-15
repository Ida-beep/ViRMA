using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;

public class ViRMA_PocketGuideFormat : MonoBehaviour
{
    public GameObject titleField;
    public GameObject textField;
    //public GameObject video;

    void Update()
    {
        // Specify position of title and text
        RectTransform textTransform = textField.GetComponent<RectTransform>();
        textTransform.localRotation =  Quaternion.Euler(180, 180, 0);
    }

    public void SetText(string newTitle, string newInstruction){
        // Specify text-field of each gameobject
        textField.GetComponent<TMPro.TextMeshProUGUI>().text = newInstruction;
        titleField.GetComponent<TMPro.TextMeshProUGUI>().text = newTitle;
        //Debug.Log("title = " + newTitle + " , and instruction is = " + newInstruction);
    }

    public void SetVideo(UnityEngine.Video.VideoClip newVideoPath, UnityEngine.Video.VideoPlayer vp){
        //var vp = video.GetComponent<UnityEngine.Video.VideoPlayer>(); 
        //vp.source = VideoSource.Url;
        vp.clip = newVideoPath;
        vp.playOnAwake = true;
        vp.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        vp.targetMaterialRenderer = GetComponent<Renderer>();
        //vp.targetMaterialPro perty = "_MainTex";
        vp.isLooping = true;
        vp.Play();
    }

}
