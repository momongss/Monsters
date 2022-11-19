using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public SkyGunAim gun_25mm;
    public SkyGunAim gun_40mm;
    public SkyGunAim gun_105mm;

    SkyGunAim[] gunList;

    public CinemachineVirtualCamera cam_25mm;
    public CinemachineVirtualCamera cam_40mm;
    public CinemachineVirtualCamera cam_105mm;

    CinemachineVirtualCamera[] camList;

    private void Awake()
    {
        gunList = new SkyGunAim[3];

        gunList[0] = gun_25mm;
        gunList[1] = gun_40mm;
        gunList[2] = gun_105mm;

        gunList[1].enabled = false;
        gunList[2].enabled = false;

        camList = new CinemachineVirtualCamera[3];

        camList[0] = cam_25mm;
        camList[1] = cam_40mm;
        camList[2] = cam_105mm;

        camList[0].Priority = 11;
        camList[1].Priority = 10;
        camList[2].Priority = 10;
    }

    public void ChangeGun(int bulletType)
    {
        for (int i = 0; i < gunList.Length; i++)
        {
            if (i == bulletType)
            {
                gunList[i].enabled = true;

                camList[i].Priority = 11;
            } else
            {
                gunList[i].enabled = false;
                camList[i].Priority = 10;
            }
        }
    }
}
