using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIWin : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Text coinText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI visitorText;
    [SerializeField] Text curCoinText;

    [SerializeField] Button nextLevelButton;
    [SerializeField] Button homeButon;
    [SerializeField] Button adsButon;

    [SerializeField] Transform titleTF;
    [SerializeField] Transform glowFX;
    [SerializeField] SpinData[] spinDatas;
    [SerializeField] float spinTotalTime;
    [SerializeField] Transform arrowTF;
    [SerializeField] RectTransform[] coinFXs;
    [SerializeField] RectTransform coinIconRT;

    private float spinTotalRate;
    private float spinTime;
    private float spinSpeed = 1;
    private float spinDir = 1;
    private bool isSpinning = true;
    private int curSpinIdx;


    private void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevelButton);
        homeButon.onClick.AddListener(HomeButton);
        adsButon.onClick.AddListener(() =>
        {
            isSpinning = false;
            if (curSpinIdx == 0)
            {
                arrowTF.DOKill();
                arrowTF.DOLocalRotate(new Vector3(0, 0, 80), 0.2f).SetEase(Ease.Linear);
            }
            if (curSpinIdx == spinDatas.Length - 1)
            {
                arrowTF.DOKill();
                arrowTF.DOLocalRotate(new Vector3(0, 0, -80), 0.2f).SetEase(Ease.Linear);
            }
            
            SetCoinText(GetReward() * spinDatas[curSpinIdx].multiplier);
            var coin = GetReward() * (spinDatas[curSpinIdx].multiplier - 1);
            GameManager.Instance.AddCoin(coin);
            UpdateUserCoin();
            StartCoroutine(ShowFX());
            // AdUtil.ShowRewardedAd(() =>
            // {
            //     SetCoinText(GetReward() * spinDatas[curSpinIdx].multiplier);
            //     var coin = GetReward() * (spinDatas[curSpinIdx].multiplier - 1);
            //     GameManager.Instance.AddCoin(coin);
            //     UpdateUserCoin();
            //     StartCoroutine(ShowFX());
            // });
            adsButon.interactable = false;
        });
        foreach (var dat in spinDatas) spinTotalRate += dat.portion;
    }

    private IEnumerator ShowFX()
    {
        foreach(var rt in coinFXs)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.gameObject.SetActive(true);
            rt.DOMove(coinIconRT.position, 0.6f).OnComplete(() => rt.gameObject.SetActive(false));
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        NextLevelButton();
    }

    private void OnDisable()
    {
        foreach (var rt in coinFXs)
        {
            rt.DOKill();
            rt.gameObject.SetActive(false);
        }
    }

    private int GetReward()
    {
        return GameHelper.GetClearReward(DayManager.Instance.ServedPassenger);
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        glowFX.Rotate(Vector3.forward, dt * 20, Space.Self);

        if (isSpinning) UpdateSpin(dt);
    }

    private void UpdateSpin(float dt)
    {
        spinTime += dt * spinDir * spinSpeed * 0.5f;
        if (spinTime >= spinTotalTime)
        {
            spinTime = spinTotalTime - dt;
            spinDir = -1;
        }
        else if (spinTime <= 0)
        {
            spinTime = dt;
            spinDir = 1;
        }

        var t = spinTime / spinTotalTime;
        var from = 90f;
        var angleZ = Mathf.Lerp(from, -from, t);
        arrowTF.localEulerAngles = new Vector3(0, 0, angleZ);

        var length = spinDatas.Length;
        spinSpeed = spinDatas[0].multiplier;
        for (var i = 0; i < length; i++)
        {
            var to = spinDatas[i].angle;
            if (angleZ <= from && angleZ >= to)
            {
                spinSpeed = spinDatas[i].multiplier;
                curSpinIdx = i;
                break;
            }
            from = to;
        }
    }

    public override void Show()
    {
        base.Show();
        isSpinning = true;
        spinTime = 0f;
        spinSpeed = 1f;
        SetCoinText(GetReward());
        SetVisitorText(DayManager.Instance.TotalDayPassenger);

        if (DayManager.Instance.UseDayInUserData)
            GameManager.Instance.UserData.day = DayManager.Instance.DayIndex + 1;
        GameManager.Instance.AddCoin(GetReward());
        homeButon.gameObject.SetActive(DayManager.Instance.DayIndex >= DayManager.Instance.dayConfig.licenceDay);
        Game.Save();
        UpdateUserCoin();

        adsButon.interactable = true;
        titleTF.localScale = Vector3.zero;
        titleTF.DOScale(Vector3.one, 0.5f).SetDelay(0.1f);
        //adsButon.interactable = AdUtil.IsAdsReady();
    }

    private void UpdateUserCoin()
    {
        curCoinText.text = GameManager.Instance.UserData.coin.ToString();
    }

    public void NextLevelButton()
    {
        var dat = GameManager.Instance.UserData;
        if (!dat.hasUnlockLicense && dat.day == DayManager.Instance.dayConfig.licenceDay)
        {
            HomeButton();
        }
        else
        {
            Hide();
            GameManager.Instance.ChangeState(GameStates.NextLevel);
        }
    }

    public void HomeButton()
    {
        Hide();
        GameManager.Instance.ChangeState(GameStates.Home);
    }

    private void SetCoinText(float coin)
    {
        coinText.text = coin.ToString();
    }

    private void SetDayText(float day)
    {
        dayText.text = "Day " + day.ToString();
    }

    private void SetVisitorText(float v)
    {
        visitorText.text = v.ToString();
    }

    [System.Serializable]
    private class SpinData
    {
        public Image image;
        public int multiplier;
        public float portion;
        public float angle;
    }

}
