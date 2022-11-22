using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;


public class Squash : MonoBehaviour
{
    public MMSquashAndStretch squashAndStretch;
    public float duration = 0.5f;
    public float intensity = 1f;

    public void Play()
    {
        squashAndStretch.Squash(duration, intensity);
    }
}
