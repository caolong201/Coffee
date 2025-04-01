using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : ButtonAssistant
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

        foreach (var item in GameController.Instance.ItemList)
        {
            item.Deselect();
        }

        GameController.Instance.ResetHint();

        GameController.Instance.DestroyeMode = !GameController.Instance.DestroyeMode;

        if(GameController.Instance.DestroyeMode)
        {
            UIController.Instance.BombLabel.SetActive(true);
        }
        else
        {
            UIController.Instance.BombLabel.SetActive(false);
        }
    }
}
