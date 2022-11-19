using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool triggerPressed;

    float bulletSpread = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            triggerPressed = true;
        } else if (Input.GetKeyUp(KeyCode.A))
        {
            triggerPressed = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        triggerPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        triggerPressed = false;
    }
}
