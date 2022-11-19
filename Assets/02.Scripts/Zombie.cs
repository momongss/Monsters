using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum State { Move, Attack, Die, Stunned };
    public State state;

    [Header("능력치")]
    public float maxHP;
    public float hp;
    public float moveSpeed = 10f;

    [Header("컴포넌트")]
    public Animator anim;
    public Rigidbody rigid;
    public Transform destination;
    
    ZombieTroop zombieTroop;

    private void Awake()
    {
        zombieTroop = transform.parent.GetComponent<ZombieTroop>();
        zombieTroop.AddZombie(this);

        transform.parent = null;

        rigid.centerOfMass = new Vector3(0, 1f, 0);
    }

    void Start()
    {
        destination = Place_ZombieTarget.I.transform;
        hp = maxHP;
        ChangeState(State.Move);
    }

    void ChangeState(State _state)
    {
        if (state == State.Die) return;

        state = _state;

        switch (state)
        {
            case State.Move:
                anim.SetTrigger("Run");
                break;
            case State.Attack:
                anim.SetTrigger("Attack0");
                break;
            case State.Stunned:
                break;
        }
    }

    IEnumerator ChangeState(State _state, float delay)
    {
        yield return new WaitForSeconds(delay);

        state = _state;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Move:
                Move();
                break;
            case State.Attack:
                break;
            case State.Stunned:
                Collider coll = Utils.RayCast(transform.position, -Vector3.up, 1f);
                if (coll)
                {
                    ChangeState(State.Move);
                }
                break;
        }
    }

    void Move()
    {
        transform.LookAt(destination);
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
    }

    public void OnDamaged(float damage, Vector3 origin, float explosionForce, float stunTime = 0f)
    {
        hp -= damage;

        ChangeState(State.Stunned);

        if (hp <= 0)
        {
            hp = 0;
            OnDie(origin, explosionForce);
        }
    }

    public void OnDie(Vector3 origin, float explosionForce)
    {
        ChangeState(State.Die);

        zombieTroop.RemoveZombie(this);

        Vector3 v = transform.position - origin;
        v = v.normalized;
        v.y = 5f;

        rigid.AddForce(v * explosionForce);

        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.ZombieTarget))
        {
            ChangeState(State.Attack);
        }
    }
}
