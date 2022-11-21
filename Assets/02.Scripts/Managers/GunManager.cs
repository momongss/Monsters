using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Gun gun_25mm;
    public Gun gun_40mm;
    public Gun gun_105mm;

    Gun[] gunList;

    private void Awake()
    {
        gunList = new Gun[3];

        gunList[0] = gun_25mm;
        gunList[1] = gun_40mm;
        gunList[2] = gun_105mm;

        gunList[1].enabled = false;
        gunList[2].enabled = false;
    }

    public void ChangeGun(int bulletType)
    {
        for (int i = 0; i < gunList.Length; i++)
        {
            if (i == bulletType)
            {
                gunList[i].enabled = true;
            } else
            {
                gunList[i].enabled = false;
            }
        }
    }
}
