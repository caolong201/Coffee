using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum ETutorialType
{
    HighlightItem,
    Step1,
    Step2,
    Done
}

public class UITutorialMainGame : Singleton<UITutorialMainGame>
{
    public ETutorialType TutorialType = ETutorialType.Done;
    [SerializeField] private List<GameObject> uis;
    private void Start()
    {
        if (UserData.LevelIndex > 0)
        {
            TutorialType = ETutorialType.Done;
            gameObject.SetActive(false);
        }
    }

    public void SetStep(ETutorialType tutorialType)
    {
        Debug.LogError(tutorialType);
        TutorialType = tutorialType;
        for (int i = 0; i < uis.Count; i++)
        {
            if (uis[i] != null)
            {
                uis[i].SetActive(false);
            }
        }
        
        if(TutorialType == ETutorialType.Done) return;
        
        uis[(int)TutorialType].SetActive(true);

        if (TutorialType == ETutorialType.Step2)
        {
            DOVirtual.DelayedCall(2, () =>
            {
                uis[(int)TutorialType].SetActive(false);
                TutorialType = ETutorialType.Done;
                gameObject.SetActive(false);
            });
        }
    }

}
