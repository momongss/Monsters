using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamRotateTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float rotateSpeed = 1f;

    bool isRotating = false;

    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (isRotating && Input.touchCount > 0)
        {
            Touch touch = new Touch();
            bool isValidTouch = false;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                if (t.position.x >= Screen.width * 0.5f)
                {
                    touch = t;
                    isValidTouch = true;
                    break;
                }
            }

            if (isValidTouch == false) return;

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 direction = touch.deltaPosition;
                Vector3 moveDir = (GetCamUp() * direction.y + GetCamRight() * direction.x).normalized * rotateSpeed;

                target.Rotate(moveDir, Space.Self);
                // target.Translate(moveDir, Space.World);
            }
        }

        if (isRotating)
        {
            Vector2 currMousePos = Input.mousePosition;

            Vector2 direction = currMousePos - prevMousePos;
            Vector3 moveDir = (GetCamUp() * direction.y + GetCamRight() * direction.x).normalized * rotateSpeed;

            print(moveDir);
            target.Rotate(new Vector3(moveDir.x, moveDir.z, 0f), Space.Self);
            // target.Translate(moveDir, Space.World);

            prevMousePos = currMousePos;
        }
    }

    Vector2 prevMousePos;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        prevMousePos = Input.mousePosition;
        isRotating = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isRotating = false;
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
