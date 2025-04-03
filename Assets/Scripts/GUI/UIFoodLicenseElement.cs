using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFoodLicenseElement : MonoBehaviour
{
    [SerializeField] private Image itemIconImg;
    [SerializeField] private TMP_Text unlockPriceTxt;
    [SerializeField] private Button unlockBtn;
    [SerializeField] private Text buttonTxt;
    [SerializeField] private GameObject[] starTFs;
    [SerializeField] private Text curPriceTxt;
    [SerializeField] private Text nextPriceTxt;

    private Food food;
    public Button Button => unlockBtn;

    public void Init(Food foodConfig, Action onAction)
    {
        food = foodConfig;
        itemIconImg.sprite = food.Icon;
        Refresh();

        unlockBtn.onClick.AddListener(() =>
        {
            var data = GameManager.Instance.UserData;
            data.coin -= food.GetLicenseCost(data.GetFoodLevel(food.Id));
            GameUI.Instance.Get<UIHome>().UpdateCoin();
            if (!data.unlockedFoodIdList.Contains(food.Id))
            {
                data.UnlockFood(food.Id);
            }
            else
            {
                data.foodLevels[food.Id]++;
            }
            onAction();
        });
    }

    public void Refresh()
    {
        var data = GameManager.Instance.UserData;
        var level = data.GetFoodLevel(food.Id);
        var cost = food.GetLicenseCost(level);
        curPriceTxt.text = food.GetPrice(level).ToString();
        nextPriceTxt.text = food.GetPrice(level + 1).ToString();
        unlockPriceTxt.text = cost.ToString();
        var isUnlocked = data.unlockedFoodIdList.Contains(food.Id);
        for (var i = 0; i < starTFs.Length; i++) starTFs[i].transform.GetChild(0).SetActive(level >= i);
        buttonTxt.text = !isUnlocked ? "UNLOCK" : "UPGRADE";

        unlockBtn.interactable = cost <= GameManager.Instance.UserData.coin && level < starTFs.Length - 1;
    }
}
