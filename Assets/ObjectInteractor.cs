using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    [SerializeField] private float dragThreshold = 0.1f; // Khoảng cách tối thiểu để phân biệt click/drag
    RaycastHit hit;
    FoodItem currentSelected, lastSelected;

    void Update()
    {
        HandleMouseInput();
        HandleTouchInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                currentSelected = hit.collider.gameObject.GetComponentInParent<FoodItem>();
                if (currentSelected != null && currentSelected != lastSelected)
                {
                    currentSelected.Select();

                    if (lastSelected != null)
                    {
                        lastSelected.Deselect();
                    }

                    lastSelected = currentSelected;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && currentSelected != null)
        {
            Debug.Log($"Click: {currentSelected.name}");
            currentSelected.HandlerClick();
            currentSelected = null;
        }
    }

    // Xử lý input touch (cho mobile)
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    currentSelected = hit.collider.gameObject.GetComponentInParent<FoodItem>();
                    if (currentSelected != null && currentSelected != lastSelected)
                    {
                        currentSelected.Select();

                        if (lastSelected != null)
                        {
                            lastSelected.Deselect();
                        }

                        lastSelected = currentSelected;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended && currentSelected != null)
            {
                currentSelected.HandlerClick();
                currentSelected = null;
            }
        }
    }
}