using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieTroop : MonoBehaviour
{
    ZombieTroopManager troopManager;

    List<Zombie> zombieList = new List<Zombie>();

    private void Awake()
    {
        troopManager = transform.parent.GetComponent<ZombieTroopManager>();
        troopManager.AddTroop(this);       
    }

    public void AddZombie(Zombie zombie)
    {
        zombieList.Add(zombie);
    }

    public void RemoveZombie(Zombie zombie)
    {
        zombieList.Remove(zombie);

        if (zombieList.Count == 0)
        {
            troopManager.RemoveTroop(this);
        }
    }

    public Zombie GetRandomZombie()
    {
        int zombieNum = Random.Range(0, zombieList.Count);
        return zombieList[zombieNum];
    }

    public Zombie GetClosestZombie(Vector3 origin)
    {
        if (zombieList.Count == 0) return null;

        Zombie minDistanceZombie = zombieList[0];
        float minDistance = Vector3.Distance(
            minDistanceZombie.transform.position,
            origin
            );

        foreach (var zombie in zombieList)
        {
            float distance = Vector3.Distance(
            zombie.transform.position,
            origin
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceZombie = zombie;
            }
        }

        return minDistanceZombie;
    }
}
