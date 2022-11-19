using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCtrl : MonoBehaviour
{
    TankGroup groop;

    public ParticleSystem PS_TireLeft;
    public ParticleSystem PS_TireRight;

    Rigidbody rigid;

    private void Awake()
    {
        groop = transform.parent.GetComponent<TankGroup>();
        rigid = GetComponent<Rigidbody>();

        rigid.centerOfMass = new Vector3(0, -10f, 0);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = groop.moveDir;

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir.normalized);
            transform.Translate(Vector3.forward * Time.deltaTime * groop.moveSpeed);

            PS_TireLeft.Play();
            PS_TireRight.Play();
        } else
        {
            PS_TireLeft.Stop();
            PS_TireRight.Stop();
        }
    }
}
