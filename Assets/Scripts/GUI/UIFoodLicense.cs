using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFoodLicense : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] private UIFoodLicenseElement prefab;
    [SerializeField] private Transform root;
    [SerializeField] private FoodConfig foodConfig;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Text coinTxt;

    private bool isInited;
    private List<UIFoodLicenseElement> elements = new();

    private void Init()
    {
        isInited = true;
        for (var i = 0; i < foodConfig.FoodList.Count; i++)
        {
            var ele = Instantiate(prefab, root);
            ele.Init(foodConfig.FoodList[i], OnAction);
            elements.Add(ele);
        }
        prefab.gameObject.SetActive(false);
        closeBtn.onClick.AddListener(Hide);
    }

    public override void Hide()
    {
        base.Hide();
        Game.Save();
    }

    private void RefreshCoinLb()
    {
        coinTxt.text = GameManager.Instance.UserData.coin.ToString();
    }

    private void OnAction()
    {
        foreach (var ele in elements) ele.Refresh();
        RefreshCoinLb();
    }

    public void SetupTutorial()
    {

        var mask = GameUI.Instance.Get<UITutorialMask>();

        void step3()
        {
            mask.UpdateFocus(closeBtn.image.rectTransform, () =>
            {
                closeBtn.onClick.Invoke();
                GameUI.Instance.Get<UIHome>().ShowNextDayTutorial();
            });
        }

        void step2()
        {
            var fantaBtn = elements[0].Button;
            mask.UpdateFocus(fantaBtn.image.rectTransform, () =>
            {
                fantaBtn.onClick.Invoke();
                GameUI.Instance.Get<UIHome>().Show();
                step3();
            });
        }

        void step1()
        {
            var bigBurgerBtn = elements[3].Button;
            mask.UpdateFocus(bigBurgerBtn.image.rectTransform, () =>
            {
                bigBurgerBtn.onClick.Invoke();
                step2();
            });
        }
        step1();
    }

    public override void Show()
    {
        base.Show();
        if (!isInited) Init();
        OnAction();
    }
}
