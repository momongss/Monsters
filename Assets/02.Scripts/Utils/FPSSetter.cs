using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSetter : MonoBehaviour
{
    public int fps = 120;

    private void Awake()
    {
        Application.targetFrameRate = fps;
    }
}
