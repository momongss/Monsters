using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTroopManager : MonoBehaviour
{
    public static ZombieTroopManager I;

    public List<ZombieTroop> zombieTroops = new List<ZombieTroop>();

    private void Awake()
    {
        I = this;
    }

    public void RemoveTroop(ZombieTroop troop)
    {
        zombieTroops.Remove(troop);

        if (zombieTroops.Count == 0)
        {
            BattleManager.I.OnBattleEnd(true);
        }
    }

    public void AddTroop(ZombieTroop troop)
    {
        zombieTroops.Add(troop);
    }

    public ZombieTroop GetClosestTroop(Vector3 tankTroopPos)
    {
        if (zombieTroops.Count == 0) return null;

        ZombieTroop minDistanceTroop = zombieTroops[0];
        float minDistance = Vector3.Distance(
            minDistanceTroop.transform.position,
            tankTroopPos
            );

        foreach (var troop in zombieTroops)
        {
            float distance = Vector3.Distance(
            troop.transform.position,
            tankTroopPos
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceTroop = troop;
            }
        }

        return minDistanceTroop;
    }
}
