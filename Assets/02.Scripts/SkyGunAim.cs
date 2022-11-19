using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SkyGunAim : MonoBehaviour
{
    public Bullet.Type bulletType;

    public Transform Tr_muzzle;
    public TriggerButton triggerButton;

    public float prevFireTime = 0f;

    public float fireRate = 0.1f;

    public float max_FireSpread = 2f;
    public float fireSpreadUp = 0.1f;
    public float fireSpreadDown = 0.1f;

    public float fireForce = 800f;

    public float spread = 0f;

    protected virtual void Start()
    {
        prevFireTime = Time.time;
    }

    protected virtual void Fire()
    {
        float currTime = Time.time;
        if (currTime - prevFireTime >= fireRate)
        {
            Bullet bullet = BulletPoolManager.I.GetBullet(bulletType);

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
