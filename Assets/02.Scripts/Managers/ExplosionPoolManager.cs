using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class ExplosionPoolManager : MonoBehaviour
{
    public static ExplosionPoolManager I { get; private set; }

    public PoolManager poolManager_25mm;
    public PoolManager poolManager_40mm;
    public PoolManager poolManager_105mm;

    public int level_25mm = 0;
    public int level_40mm = 0;
    public int level_105mm = 0;

    PoolManager[] poolManagers;
    int[] levels;

    private void Awake()
    {
        I = this;

        poolManagers = new PoolManager[3];
        poolManagers[0] = poolManager_25mm;
        poolManagers[1] = poolManager_40mm;
        poolManagers[2] = poolManager_105mm;

        levels = new int[3];
        levels[0] = level_25mm;
        levels[1] = level_40mm;
        levels[2] = level_105mm;
    }

    public Explosion GetItem(Bullet.Type type)
    {
        return poolManagers[(int)type].GetFromPool<Explosion>(levels[(int)type]);
    }

    public void ReturnPool(Explosion explosion)
    {
        poolManagers[(int)explosion.type].TakeToPool((int)explosion.type, explosion);
    }
}
