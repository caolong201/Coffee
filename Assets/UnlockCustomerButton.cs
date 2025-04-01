using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCustomerButton : ButtonAssistant
{
    public GuessController _controller;

    protected override void OnClick()
    {
        base.OnClick();

        if(UserData.TotalMoneyInGame >= _controller.Cost)
        {
            UserData.TotalMoneyInGame -= _controller.Cost;
            UserData.CustomerLevel++;

            _controller.Unlock();
        }
    }
}
