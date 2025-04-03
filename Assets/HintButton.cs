using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButton : ButtonAssistant
{
    private void Start()
    {
        //if (UserData.LevelIndex <= 1 && UserData.OrderIndex == 0)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    protected override void OnClick()
    {
        base.OnClick();
        if(UITutorialMainGame.Instance.TutorialType != ETutorialType.Done) return;
        
        GameController.Instance.DestroyeMode = false;
        UIController.Instance.BombLabel.SetActive(false);

        GameController.Instance.Hint();
    }
}
