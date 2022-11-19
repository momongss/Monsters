using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    public enum Type { 
        Bullet_25 = 0,
        Bullet_40 = 1,
        Bullet_105 = 2,

        Bullet_25_1 = 4,
        Bullet_40_1 = 4,
        Bullet_105_1 = 4,
    }

    public Type type;

    public float damage = 10f;

    public Rigidbody rigid;

    public Explosion explosion;
    
    public float spread;
    public float startTime;
    public float endTime;

    public string ID_pool;

    [SerializeField] ParticleSystem PS_Hit;
    [SerializeField] AudioSource sound;

    public void Fire(Vector3 initPos, Quaternion initRot, float fireForce)
    {
        sound.Play();

        transform.position = initPos;
        transform.rotation = initRot;

        rigid.AddForce(transform.forward * fireForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Monster))
        {
            MonsterHP m = other.GetComponent<MonsterHP>();
            m.OnDamaged(damage);
        }

        rigid.velocity = Vector3.zero;

        Explosion explosion = ExplosionPoolManager.I.GetItem(type);
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        explosion.Play();

        sound.Stop();
        BulletPoolManager.I.ReturnPool(this);
    }

    public void OnCreatedInPool()
    {

    }

    public void OnGettingFromPool()
    {
        
    }
}
