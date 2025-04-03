using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => true;

    public override bool UseBehindPanel => true;

    [SerializeField] Button retryButton;
    [SerializeField] Button retryAdsButton;
    [SerializeField] Button homeButton;
    [SerializeField] GameObject failedText;
    [SerializeField] Text dayTxt;
    [SerializeField] Text costTxt;

    private int replayCost = 100;

    private void Start()
    {
        costTxt.text = replayCost.ToString();
        retryButton.onClick.AddListener(RetryCoin);
        retryAdsButton.onClick.AddListener(RetryAds);
        homeButton.onClick.AddListener(HomeButton);
    }

    private void RetryCoin()
    {
        GameManager.Instance.ChangeState(GameStates.Resume);
        GameManager.Instance.AddCoin(-replayCost);
        Hide();
        GameUI.Instance.Get<UIInGame>().UpdateCoinLb();
    }

    private void RetryAds()
    {
        GameManager.Instance.ChangeState(GameStates.Resume);
        Hide();
        // AdUtil.ShowRewardedAd(() =>
        // {
        //     GameManager.Instance.ChangeState(GameStates.Resume);
        //     Hide();
        // });
    }

    public void HomeButton()
    {
        if (DayManager.Instance.HasShop())
        {
            DayManager.Instance.CleanPassengers();
            DayManager.Instance.Clear(true);
            DayManager.Instance.ReduceDay();
            GameManager.Instance.ChangeState(GameStates.Home);
            DayManager.Instance.RemoveLast();

        }
        else
        {
            GameManager.Instance.ChangeState(GameStates.Retry);
        }
        Hide();
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.AddCoin(-DayManager.Instance.DayCoin);
        VFXAnimationManager.Instance.PulsingAnimation(failedText, new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, 0.5f);
        retryButton.gameObject.SetActive(false);
        //homeButton.gameObject.SetActive(false);
        
        StartCoroutine(ShowRetryButton());
        retryButton.interactable = GameManager.Instance.UserData.coin >= replayCost;
        //retryAdsButton.interactable = AdUtil.IsAdsReady();
        //dayTxt.text = $"Day {DayManager.Instance.DayIndex + 1}";
        Game.Save();
    }

    IEnumerator ShowRetryButton()
    {
        //yield return new WaitForSeconds(1);
        retryButton.gameObject.SetActive(true);
        //homeButton.gameObject.SetActive(DayManager.Instance.HasShop());
        yield return null;
    }
}
