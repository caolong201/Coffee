using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeButton : ButtonAssistant
{
    protected override void OnClick()
    {
        base.OnClick();

        UserData.NewMode = !UserData.NewMode;
        UserData.IsPlayingMiniGame = false;
        SceneManager.LoadScene("Game");
    }
}
