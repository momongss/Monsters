using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager I;

    public void AddZombie(Zombie zombie)
    {

    }

    private void Awake()
    {
        I = this;
    }
}
