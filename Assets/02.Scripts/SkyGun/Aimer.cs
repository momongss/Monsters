using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    public Joystick joystick;

    public float rotSpeed = 10f;

    void Update()
    {
        float v = joystick.Vertical;
        float h = joystick.Horizontal;

        Vector3 prevRot = transform.localRotation.eulerAngles;

        transform.localRotation = Quaternion.Euler(new Vector3(
            prevRot.x - v * rotSpeed,
            prevRot.y + h * rotSpeed
            ,
            prevRot.z
            ));
    }
}
