using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_ZombieTarget : MonoBehaviour
{
    public static Place_ZombieTarget I { get; private set; }

    public float maxHP;
    public float hp;

    MMSquashAndStretch squashAndStretch;

    private void Awake()
    {
        I = this;

        squashAndStretch = GetComponent<MMSquashAndStretch>();

        hp = maxHP;
    }

    public void OnDamaged(float damage)
    {
        hp -= damage;

        squashAndStretch.Squash(0.2f, 0.2f);
    }
}
