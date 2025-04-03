using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIHome : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] private Button homeActiveButton, honeInactiveButton;
    [SerializeField] private Button shopActiveButton, shopInactiveButton;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        shopActiveButton.onClick.AddListener(PressShop);
        shopInactiveButton.onClick.AddListener(PressShop);
        homeActiveButton.onClick.AddListener(PressHome);
        honeInactiveButton.onClick.AddListener(PressHome);

        nextDayButton.onClick.AddListener(() =>
        {
            Hide();
            GameManager.Instance.ChangeState(GameStates.Start);
            GameUI.Instance.Get<UIInGame>().CheckAvailableFoodIcons();
        });
        ToHome();
    }

    public void UpdateCoin()
    {
        coinText.text = GameManager.Instance.UserData.coin.ToString();
    }

    private void ToShop() => Toggle(true);

    private void ToHome() => Toggle(false);

    private void Toggle(bool onShop)
    {
        shopActiveButton.gameObject.SetActive(onShop);
        honeInactiveButton.gameObject.SetActive(onShop);
        shopInactiveButton.gameObject.SetActive(!onShop);
        homeActiveButton.gameObject.SetActive(!onShop);
    }

    private void PressShop()
    {
        ToShop();
        GameUI.Instance.Get<UIFoodLicense>().Show();
        nextDayButton.gameObject.SetActive(false);
    }

    private void PressHome()
    {
        ToHome();
        GameUI.Instance.Get<UIFoodLicense>().Hide();
        nextDayButton.gameObject.SetActive(true);
    }

    public override void Show()
    {
        base.Show();
        var userData = GameManager.Instance.UserData;
        if (!userData.hasUnlockLicense && userData.day == DayManager.Instance.dayConfig.licenceDay)
        {
            userData.hasUnlockLicense = true;
            GameUI.Instance.Get<UITutorialMask>().Show();
            GameUI.Instance.Get<UITutorialMask>().UpdateFocus(shopInactiveButton.image.rectTransform, () =>
            {
                shopInactiveButton.onClick.Invoke();
                GameUI.Instance.Get<UIFoodLicense>().SetupTutorial();
            });
        }
        
        UpdateCoin();
        GameManager.Instance.SetupOutgame();
        dayText.text = "Day " + (userData.day + 1).ToString();
    }

    public void ShowNextDayTutorial()
    {
        GameUI.Instance.Get<UITutorialMask>().UpdateFocus(honeInactiveButton.image.rectTransform, () =>
        {
            honeInactiveButton.onClick.Invoke();

            GameUI.Instance.Get<UITutorialMask>().UpdateFocus(nextDayButton.image.rectTransform, () =>
            {
                nextDayButton.onClick.Invoke();
                GameUI.Instance.Get<UITutorialMask>().Hide();
            });
        });
    }
}
