using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeButton : ButtonAssistant
{
    public GuessController Guess;

    protected override void OnClick()
    {
        base.OnClick();

       // Guess.Serve();

        gameObject.SetActive(false);
    }
}
