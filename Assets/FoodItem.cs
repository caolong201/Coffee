using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using DG.Tweening;

public class FoodItem : MonoBehaviour
{
   
    private Vector3 offset;
    private float zCoord;
    private bool isDragging = false;
    private Vector3 initialPosition;
    private bool isClick = true;

    
    
    public int Type;
    public int Level;
    public bool IsMax;

    private bool _selected = false;

    private List<Outline> _outline = new List<Outline>();

    private float _y;

    public GameObject CheckMark { set; get; }

    public Vector3 OriginPos { set; get; }

    private Rigidbody _body;
    private Collider[] _colliders;

    private bool _isForceSelected = false;

    private void Awake()
    {
        OriginPos = transform.position;

        _colliders = GetComponentsInChildren<Collider>();
        _body = GetComponentInChildren<Rigidbody>();
    }

    public void ResetPosition()
    {
        transform.position = OriginPos;
    }

    private void OnEnable()
    {
        GameController.Instance.ItemList.Add(this);

        CheckMark = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void OnDestroy()
    {
        GameController.Instance.ItemList.Remove(this);
    }

    public void HandlerClick()
    {
        if (UITutorialMainGame.Instance.TutorialType == ETutorialType.Step1 && !_isForceSelected)
        {
            return;
        }

        if (!GameController.Instance.Moveable)
            return;

        if (GameController.Instance.DestroyeMode)
        {
            GameController.Instance.Destroy(this);
        }
        else
        {
            if (!_selected && GameController.Instance.SelectedItems.Count < 7)
            {
                GameController.Instance.ResetHint();
                _selected = true;
                GameController.Instance.Select(this);
            }
        }
    }
    
    public void MoveToSlot(bool instance = false)
    {
        foreach (var item in _colliders)
        {
            item.enabled = false;
        }

        _body.isKinematic = true;

        if (!instance)
        {
            Select();
            transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f);
            transform.DOScale(Vector3.one * 0.8f, 0.3f);
            transform.DOLocalMove(
                new Vector3(transform.localPosition.x / 1.2f, transform.localPosition.y / 1.2f,
                    transform.localPosition.z + 1f), 0.2f).SetEase(Ease.OutCubic).OnComplete(
                () =>
                {
                    Deselect();
                    transform.DOScale(Vector3.one * 0.2f, 0.25f);
                    transform.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
                    {
                        transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
                    });
                });
        }
        else
        {
            transform.localScale = Vector3.one * 0.2f;
            transform.localEulerAngles = new Vector3(-90, 0, 0);
            transform.localPosition = Vector3.zero;
        }
    }

    private void GetOutline()
    {
        _outline.Add(transform.GetChild(0).gameObject.AddComponent<Outline>());

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            _outline.Add(transform.GetChild(0).GetChild(i).gameObject.AddComponent<Outline>());
        }
    }
    
    public void ForceSelect()
    {
        _isForceSelected = true;
        Select();
    }

    public void Select()
    {
        
        if (UITutorialMainGame.Instance.TutorialType == ETutorialType.Step1 && !_isForceSelected)
        {
            return;
        }
        
        if (_outline.Count == 0)
            GetOutline();

        if (_outline.Count > 0)
        {
            foreach (var item in _outline)
            {
                item.enabled = true;
                item.color = 3;
            }
        }
    }

    public void Deselect()
    {
        _selected = false;

        if (_outline.Count == 0)
            GetOutline();

        if (_outline.Count > 0)
        {
            foreach (var item in _outline)
            {
                if (item)
                    item.enabled = false;
            }
        }
    }
}