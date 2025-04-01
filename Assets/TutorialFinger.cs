using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialFinger : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
