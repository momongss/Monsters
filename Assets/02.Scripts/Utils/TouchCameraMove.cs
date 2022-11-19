using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraMove : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    public Rigidbody target;

    [SerializeField] RectTransform[] UI_list;

    public float brake = 1f;
    public float ss = 0.5f;

    void Brake(float factor = 1f)
    { 
        target.AddForce(-target.velocity * brake * factor);
    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            Brake(0.2f);
        } else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (isTouchOnUI(touch))
            {
                return;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 direction = touch.deltaPosition;
                Vector3 moveDir = (GetCamUp() * direction.y + GetCamRight() * direction.x).normalized * moveSpeed;

                target.transform.Translate(moveDir, Space.World);
            } else if (touch.phase == TouchPhase.Stationary)
            {
                Brake();
            }
        } else if (Input.touchCount >= 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(0);

            bool isUI0 = isTouchOnUI(touch0);
            bool isUI1 = isTouchOnUI(touch1);

            Touch touch;

            if (isUI0 && isUI1)
            {
                return;
            } else if (isUI0)
            {
                touch = touch0;
            } else if (isUI1)
            {
                touch = touch1;
            } else
            {
                touch = touch0;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 direction = touch.deltaPosition;
                Vector3 moveDir = (GetCamUp() * direction.y + GetCamRight() * direction.x).normalized * moveSpeed;

                target.transform.Translate(moveDir, Space.World);
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                Brake();
            }
        }
    }

    bool isTouchOnUI(Touch touch)
    {
        return (EventSystem.current.IsPointerOverGameObject(touch.fingerId));
    }

    Vector3 GetCamUp()
    {
        Vector3 camUp = Camera.main.transform.up;
        camUp.y = 0;

        camUp = camUp.normalized;

        return camUp;
    }

    Vector3 GetCamRight()
    {
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;

        camRight = camRight.normalized;

        return camRight;
    }
}
