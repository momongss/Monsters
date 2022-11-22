using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWeaponSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public SlotData slotData;

    bool isDragging = false;

    [SerializeField] RectTransform rect;

    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        foreach (UIWeaponSlot s in UIWeaponSlotManager.I.slotList)
        {
            if (s != this)
            {
                s.image.raycastTarget = false;
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isDragging == false) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;


        foreach (UIWeaponSlot s in UIWeaponSlotManager.I.slotList)
        {
            s.image.raycastTarget = true;
        }
    }
}
