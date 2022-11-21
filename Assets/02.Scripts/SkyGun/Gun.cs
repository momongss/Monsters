using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum Type { 
        G25mm = 0, 
        G40mm = 1, 
        G105mm = 2 
    };

    public Type type;

    public Transform Tr_muzzle;
    public TriggerButton triggerButton;

    public float prevFireTime = 0f;

    public float fireRate = 0.1f;

    public float max_FireSpread = 2f;
    public float fireSpreadUp = 0.1f;
    public float fireSpreadDown = 0.1f;

    public float fireForce = 800f;

    public float spread = 0f;

    int level;

    [SerializeField] PoolManager poolManager;

    private void Awake()
    {
        InitDatas();
        InitStats();

        print(Application.persistentDataPath);
    }

    void InitStats()
    {
        if (poolManager == null) poolManager = GetComponentInChildren<PoolManager>();

        IPool<Bullet> pool = poolManager.GetPool<Bullet>(0);
        Bullet bullet = pool.Source;

        bullet.damage = GunStats.damage[(int)type][level];
        bullet.shootedGun = this;

        fireRate = GunStats.fireRate[(int)type][level];
    }

    void InitDatas()
    {
        string dataPath = $"/Gun_{(int)type}.json";

        if (JsonData.isFileExist(dataPath) == false)
        {
            level = 0;
            JsonData.SaveJson($"{level}", dataPath);
        } else
        {
            level = int.Parse(JsonData.LoadJson(dataPath));
        }
    }

    protected virtual void Start()
    {
        prevFireTime = Time.time;
    }

    protected virtual void Fire()
    {
        float currTime = Time.time;
        if (currTime - prevFireTime >= fireRate)
        {
            Bullet bullet = poolManager.GetFromPool<Bullet>();
            // Bullet bullet = BulletPoolManager.I.GetBullet(bulletType);

            Vector3 spreadVector = new Vector3(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
            );

            Vector3 rot = Tr_muzzle.eulerAngles + spreadVector;

            bullet.Fire(Tr_muzzle.position, Quaternion.Euler(rot), fireForce);

            prevFireTime = currTime;
        }
    }

    public void Return(Bullet bullet)
    {
        poolManager.TakeToPool<Bullet>(bullet);
    }

    protected virtual void UnFire()
    {

    }

    void Update()
    {
        if (triggerButton.triggerPressed)
        {
            Fire();

            if (spread < max_FireSpread)
            {
                spread += fireSpreadUp;
            } else
            {
                spread = max_FireSpread;
            }
        } else
        {
            UnFire();
            if (spread > 0)
            {
                spread -= fireSpreadDown;
            } else
            {
                spread = 0f;
            }
        }
    }
}
