using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    public float maxHP;
    public float hp;
    SquashNStretch squashNStretch;

    private void Awake()
    {
        squashNStretch = GetComponent<SquashNStretch>();
    }

    private void Start()
    {
        hp = maxHP;
    }

    public void OnDamaged(float damage)
    {
        squashNStretch.Squash_N_Stretch(1.2f, 0.8f, 1.2f);

        hp -= damage;
        if (hp <= 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        squashNStretch.Squash_N_Stretch(() =>
        {
            Destroy(gameObject);
        }, 1.4f, 0.3f, 1.4f);
    }
}
