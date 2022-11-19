using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyRotate : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    CinemachineTrackedDolly dolly;

    public float rotateSpeed = 1f;

    private void Awake()
    {
        dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        dolly.m_PathPosition += rotateSpeed * Time.deltaTime;
    }
}
