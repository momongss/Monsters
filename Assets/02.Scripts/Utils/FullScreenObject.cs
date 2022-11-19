using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenObject : MonoBehaviour
{
    public RectTransform rect;

    // Start is called before the first frame update
    void Awake()
    {
        float width = Screen.width;
        float height = Screen.height;

        rect.sizeDelta = new Vector2(width, height);
    }
}
