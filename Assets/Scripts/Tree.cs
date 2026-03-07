using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tree : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform elementParent;
    
    private Vector2 screenSize;
    private Vector2 beforePointerPosition;
    private bool isMouseDown = false;


    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        GoCenter();
    }

    public void GoCenter()
    {
        elementParent.anchoredPosition = screenSize / 2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        isMouseDown = true;
        beforePointerPosition = eventData.position;
        
        Debug.Log(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        isMouseDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right || !isMouseDown)
            return;
     
        Vector2 diff = eventData.position - beforePointerPosition;
        elementParent.anchoredPosition += diff;
        
        // 움직일 수 있는 범위 제한
        if (elementParent.anchoredPosition.x > screenSize.x)
            elementParent.anchoredPosition = new Vector2(screenSize.x, elementParent.anchoredPosition.y);
        if (elementParent.anchoredPosition.x < 0)
            elementParent.anchoredPosition = new Vector2(0, elementParent.anchoredPosition.y);
        if (elementParent.anchoredPosition.y > screenSize.y)
            elementParent.anchoredPosition = new Vector2(elementParent.anchoredPosition.x, screenSize.y);
        if (elementParent.anchoredPosition.y < 0)
            elementParent.anchoredPosition = new Vector2(elementParent.anchoredPosition.x, 0);
        
        beforePointerPosition = eventData.position;
    }
}
