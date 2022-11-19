using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterCtrl : MonoBehaviour
{
    enum AnimID { 
        Idle = 1,
        Move = 2,
        Attack = 3,
        Damage = 4,
        Die = 5 
    };

    public enum State { Idle, Move, Attack }
    public State state;

    public float moveSpeed = 5f;
    public Transform target;

    public float damage = 10f;
    public float attackDuration = 1.5f;

    Animator anim;

    IEnumerator Start()
    {
        target = Place_ZombieTarget.I.transform;

        anim = GetComponent<Animator>();

        yield return new WaitForSeconds(1f);

        StartCoroutine(SetState(State.Move));
    }

    IEnumerator SetState(State newState)
    {
        state = newState;
        yield return null;

        switch (state)
        {
            case State.Idle:
                anim.SetInteger("animation", (int)AnimID.Idle);
                break;
            case State.Move:
                anim.SetInteger("animation", (int)AnimID.Idle);
                yield return new WaitForSeconds(0.5f);
                anim.SetInteger("animation", (int)AnimID.Move);
                break;
            case State.Attack:
                anim.SetInteger("animation", (int)AnimID.Idle);
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(AttackRoutine());
                anim.SetInteger("animation", (int)AnimID.Attack);
                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        while (state == State.Attack)
        {
            yield return new WaitForSeconds(attackDuration);
            Place_ZombieTarget.I.OnDamaged(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Move:
                Move();
                break;
            case State.Attack:
                break;
        }
    }

    void Move()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.ZombieTarget))
        {
            StartCoroutine(SetState(State.Attack));
        }
    }
}
