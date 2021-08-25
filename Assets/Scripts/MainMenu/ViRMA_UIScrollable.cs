﻿using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViRMA_UIScrollable : MonoBehaviour
{
    private ViRMA_GlobalsAndActions globals;
    private ScrollRect scrollRect;
    private bool allowScrolling;

    private void Awake()
    {
        globals = Player.instance.gameObject.GetComponent<ViRMA_GlobalsAndActions>();
        scrollRect = GetComponent<ScrollRect>();

        SetMenuColliderSize();
    }

    private void Start()
    {
        GameObject btnParent = transform.GetChild(0).gameObject;
        GameObject testBtn = btnParent.transform.GetChild(0).gameObject;
        testBtn.name = "Button 1";
        for (int i = 0; i < 9; i++)
        {
            GameObject newBtn = Instantiate(testBtn, btnParent.transform);
            string btnName = "Button " + (i + 2);
            newBtn.name = btnName;
            newBtn.GetComponentInChildren<Text>().text = btnName;
        }
    }

    private void Update()
    {
        EnableJoystickTouchScrolling();
    }

    private void EnableJoystickTouchScrolling()
    {
        if (allowScrolling)
        {
            float joyStickDirection = globals.menuInteraction_Scroll.GetAxis(SteamVR_Input_Sources.Any).y;
            if (joyStickDirection != 0)
            {
                float multiplier = joyStickDirection * 10f;
                scrollRect.verticalNormalizedPosition = (scrollRect.verticalNormalizedPosition + multiplier * Time.deltaTime);
            }
        }
    }

    private void SetMenuColliderSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        BoxCollider menuCol = GetComponent<BoxCollider>();
        menuCol.size = new Vector3(width, height, 25);
    }

    private void OnTriggerStay(Collider triggeredCol)
    {
        if (triggeredCol.GetComponent<ViRMA_Drumstick>())
        {
            allowScrolling = true;
        }
    }

    private void OnTriggerExit(Collider triggeredCol)
    {
        if (triggeredCol.GetComponent<ViRMA_Drumstick>())
        {
            allowScrolling = false;
        }
    }
}
