// http://stackoverflow.com/questions/11497085/move-camera-over-terrain-using-touch-input-in-unity-3d/27836741#27836741

using UnityEngine;
using System.Collections;

public class ViewDrag : MonoBehaviour
{
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;
        } else if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            LeftMouseDrag();
        } else if (Input.GetMouseButtonUp(0))
        {
            direction *= damping;
            transform.position = transform.position - direction;
        }
    }

    Vector3 direction;
    public float damping = 0.01f;

    void LeftMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y;

        direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        transform.position = camera_position - direction;
    }
}