using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Bullet.Type type;

    public float damage;
    public float force;
    public ParticleSystem PS_explosion;
    public Collider coll;

    [SerializeField] AudioSource sound;

    public void Play()
    {
        PS_explosion.Play();
        sound.Play();

        coll.enabled = true;
        StartCoroutine(DisableCollider());
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.1f);
        coll.enabled = false;

        yield return new WaitForSeconds(6f);
        ExplosionPoolManager.I.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Monster))
        {
            print("int");
            MonsterHP m = other.GetComponent<MonsterHP>();
            m.OnDamaged(damage);
        }
    }
}
