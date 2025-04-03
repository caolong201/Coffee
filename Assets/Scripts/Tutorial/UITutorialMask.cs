using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialMask : UIElement
{
    [SerializeField] private new Camera camera;
    [SerializeField] private RectTransform mainRT;
    [SerializeField] private GameObject focusMask;
    [SerializeField] private Button button;
    [SerializeField] private RectTransform handRT;

    private Material focusMat;
    private RectTransform canvasRT;
    private float t;
    private bool isInit;

    public override bool ManualHide => true;
    public override bool DestroyOnHide => true;
    public override bool UseBehindPanel => false;

    private void Init()
    {
        isInit = true;
        var img = focusMask.GetComponent<Image>();
        focusMat = img.material;
        canvasRT = transform.parent as RectTransform;

        VFXAnimationManager.Instance.PulsingAnimation(handRT.GetChild(0).GetChild(0).gameObject, new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, 0.5f);
    }

    public override void Show()
    {
        if (!isInit) Init();
        base.Show();
    }

    public void UpdateFocus(RectTransform rt, Action action)
    {
        IEnumerator delay()
        {
            yield return null;
            yield return null;
            yield return null;
            DoUpdateFocus(rt, action);
        }
        StartCoroutine(delay());
    }

    private void DoUpdateFocus(RectTransform rt, Action action)
    {
        handRT.gameObject.SetActive(true);
        focusMask.SetActive(true);

        var mainSize = mainRT.rect;
        var width = mainSize.width;
        var height = mainSize.height;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });

        var anchoredPos = ViewportPosToCanvasPos(rt);
        var size = rt.sizeDelta;
        var btnRT = button.image.rectTransform;
        btnRT.anchoredPosition = handRT.anchoredPosition = anchoredPos;
        btnRT.sizeDelta = rt.sizeDelta;
        var posX = anchoredPos.x / width + 0.5f;
        var posY = anchoredPos.y / height + 0.5f;
        var sizeX = size.x / width * 0.5f;
        var sizeY = size.y / height * 0.5f;

        var aboveFocusPoint = new Vector4(posX - sizeX, posY + sizeY, posX + sizeX, posY + sizeY);
        var belowFocusPoint = new Vector4(posX - sizeX, posY - sizeY, posX + sizeX, posY - sizeY);
        SetPoints(aboveFocusPoint, belowFocusPoint);
    }

    private void SetPoints(Vector4 top, Vector4 bottom)
    {
        focusMat.SetVector("_PointsTop", top);
        focusMat.SetVector("_PointsBottom", bottom);
    }

    public Vector2 ViewportPosToCanvasPos(RectTransform rt)
    {
        return ViewportPosToCanvasPos(GetViewportCoordinates(rt));
    }

    public Vector2 ViewportPosToCanvasPos(Vector3 ViewportPosition)
    {
        return new Vector2(
            (ViewportPosition.x * canvasRT.sizeDelta.x) - (canvasRT.sizeDelta.x * 0.5f),
            (ViewportPosition.y * canvasRT.sizeDelta.y) - (canvasRT.sizeDelta.y * 0.5f)
        );
    }

    public Vector2 GetViewportCoordinates(RectTransform uiElement)
    {
        var worldCorners = new Vector3[4];
        uiElement.GetWorldCorners(worldCorners);
        var x = (worldCorners[0].x + worldCorners[2].x) * 0.5f / Screen.width;
        var y = (worldCorners[0].y + worldCorners[2].y) * 0.5f / Screen.height;
        return new Vector2(x, y);
    }

}