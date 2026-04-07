using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragger : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform windowTransform;
    public RectTransform desktopTransform;
    private Vector2 pointerOffset;


    void Start()
    {
        GameObject go = GameObject.FindWithTag("Laptop");
        if (go != null)
        {
            desktopTransform = go.GetComponent<RectTransform>();
        }
        else
            print("Ноут не найден");

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );

        AppLayer app = windowTransform.gameObject.GetComponent<AppLayer>();
        if (app != null)
        {
            app.Show();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (windowTransform == null) return;

        Vector2 localPointerPosition;
        RectTransform canvasRect = windowTransform.parent.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            AppLayer app = windowTransform.gameObject.GetComponent<AppLayer>();
            app.anchoredPosition = Clamp(localPointerPosition - pointerOffset);
        }
    }

    private Vector2 Clamp(Vector2 targetPos)
    {
        Vector2 minBounds = desktopTransform.rect.min - windowTransform.rect.min;
        Vector2 maxBounds = desktopTransform.rect.max - windowTransform.rect.max;

        targetPos.x = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minBounds.y + 40, maxBounds.y);

        return targetPos;
    }
}