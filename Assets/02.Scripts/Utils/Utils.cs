using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Utils
{
    public static Vector3 a(float force)
    {
        return Vector3.zero;
    }

    public static float GetRigidFallTime(float fy, float deltaTime, float gravity, float fallHeight)
    {
        float fd = (fy * deltaTime);

        float tmp = Mathf.Sqrt(fd * fd + gravity * fallHeight);

        return (tmp - fd) / gravity;
    }

    public static Vector3 RandomVectorRotation()
    {
        return new Vector3(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)
            );
    }

    public static Collider RayCast(Vector3 origin, Vector3 dir, float distance, int layerMask = ~0)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, distance, layerMask))
        {
            Debug.DrawRay(origin, dir * hit.distance, Color.yellow);

            return hit.collider;
        }
        else
        {
            Debug.DrawRay(origin, dir * distance, Color.red);

            return null;
        }
    }

    public static Transform[] GetChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }
}
