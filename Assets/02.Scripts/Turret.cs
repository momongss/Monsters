using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePos;
    public Bullet bullet;

    public float fireRate = 2f;

    public ZombieTroop targetTroop;
    public Zombie targetZombie;

    public float fireForce;

    public Transform cannon;
    public Vector3 cannonRotation;

    public float tmp;

    bool isAiming = true;

    private void Start()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (isAiming)
        {
            yield return new WaitForSeconds(fireRate);

            Bullet _bullet = Instantiate(bullet, firePos.position, firePos.rotation);

            // print($"높이 : {_bullet.transform.position.y}");

            float fallTime = Mathf.Abs(2 / Physics.gravity.y * _bullet.transform.position.y);

            float fy = fireForce * _bullet.transform.forward.y;
            float ay = fy / _bullet.rigid.mass;
            float h = _bullet.transform.position.y;

            // print($"낙하 예상 시간 : {GetFallTime(_bullet, ay * Time.deltaTime, ay, h)}");

            float d = fireForce * Time.fixedDeltaTime / _bullet.rigid.mass * Mathf.Sqrt(fallTime);

            Vector3 f = _bullet.transform.forward;

            f.y = 0;
            Vector3 pos = _bullet.transform.position + f.normalized * d;
            pos.y = 0;
        }
    }

    float GetFallTime(Bullet _bullet, float vy0, float ay, float h)
    {
        float g = Physics.gravity.y;

        return (-(Mathf.Sqrt((g * g - 6 * g * ay + ay * ay) * vy0 * vy0 + 4 * ay * ay * g * h) + (g + ay) / vy0) / (2 *ay * g));
    }

    Vector3 target = Vector3.zero;

    bool Aim()
    {
        targetTroop = ZombieTroopManager.I.GetClosestTroop(transform.position);
        if (targetTroop == null) return false;
        targetZombie = targetTroop.GetClosestZombie(transform.position);
        if (targetZombie == null) return false;

        target = targetZombie.transform.position;
        transform.LookAt(target);

        return true;
    }

    void Update()
    {
        isAiming = Aim();
    }
}
