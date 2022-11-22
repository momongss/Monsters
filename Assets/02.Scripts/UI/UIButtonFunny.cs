using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonFunny : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SquashNStretch squashNStretch;

    public void OnPointerDown(PointerEventData eventData)
    {
        squashNStretch.Squash();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        squashNStretch.Stretch();
    }
}
