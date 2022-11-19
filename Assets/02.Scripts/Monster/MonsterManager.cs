using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager I { get; private set; }

    public Transform target;

    private void Awake()
    {
        I = this;
    }
}
