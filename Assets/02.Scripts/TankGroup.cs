using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGroup : MonoBehaviour
{
    public static TankGroup Instance;

    public Transform groupOrigin;
    public Joystick joystick;

    public float moveSpeed = 10f;

    public Vector3 moveDir;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float v = joystick.Vertical;
        float h = joystick.Horizontal;

        moveDir = (GetCamUp() * v + GetCamRight() * h).normalized * moveSpeed;

        groupOrigin.Translate(moveDir * Time.deltaTime);
    }

    Vector3 GetCamUp()
    {
        Vector3 camUp = Camera.main.transform.up;

        camUp = camUp.normalized;

        return camUp;
    }

    Vector3 GetCamForward()
    {
        Vector3 camForward = Camera.main.transform.forward;

        camForward.y = 0;
        camForward = camForward.normalized;

        return camForward;
    }

    Vector3 GetCamRight()
    {
        Vector3 camRight = Camera.main.transform.right;

        camRight = camRight.normalized;

        return camRight;
    }
}
