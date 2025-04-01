using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIController : SingletonMonoBehaviour<UIController>
{
    public TextMeshProUGUI OrderLabel, StageLabel, ClearLbl;
    public Image ProgressBar;
    public UIPopupOver  UIOver;
    public UIPopupClear UIClear;
    public GameObject Hand, BombLabel;
    public GameObject[] Star;
    public Transform StarTarge;
    public UIPopupMiniGame popupMiniGame;

    private void Start()
    {
        UpdateOrder();
    }

    public void ShowStar(int idx)
    {
        var go = Instantiate(Star[idx], transform);
        go.transform.localScale = Vector3.zero;

        go.transform.DOScale(Vector3.one * 0.25f, 0.25f).SetEase(Ease.OutElastic);

        go.transform.DOMove(StarTarge.position, 0.25f).SetDelay(0.25f).OnComplete(() =>
        {
            go.transform.DOScale(Vector3.one * 0.35f, 0.075f).SetEase(Ease.Linear).OnComplete(() =>
            {
                go.transform.DOScale(Vector3.one * 0.25f, 0.075f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    DestroyImmediate(go);

                    UserData.OrderIndex++;

                    UpdateOrder();

                    if (UserData.OrderIndex >= 7)
                    {
                        UserData.OrderIndex = 0;

                        UserData.LevelIndex++;
                    }
                });
            });
        });

        go.SetActive(true);
    }

    // Update is called once per frame
    public void UpdateOrder()
    {
        //if (UserData.LevelIndex < 3)
        {
            ClearLbl.text = $"Level {UserData.LevelIndex + 1}";
            StageLabel.text = $"LEVEL {UserData.LevelIndex + 1}";
            OrderLabel.text = $"{UserData.OrderIndex}/7";

            //ProgressBar.fillAmount = UserData.OrderIndex / 7f;

            DOTween.To(() => ProgressBar.fillAmount, x => ProgressBar.fillAmount = x, UserData.OrderIndex / 7f, 0.5f);

            if (UserData.OrderIndex >= 7)
            {
                GameClear();
            }
        }
        //else
        //{
        //    ClearLbl.text = $"Level {UserData.LevelIndex + 1}";
        //    StageLabel.text = $"LEVEL {UserData.LevelIndex + 1}";
        //    OrderLabel.text = $"{UserData.OrderIndex}/7";

        //    ProgressBar.fillAmount = UserData.OrderIndex / 7f;

        //    if (UserData.OrderIndex >= 7)
        //    {
        //        GameClear();
        //    }
        //}
    }

    public void GameClear()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.GameEnd = true;
            GameController.Instance.Moveable = false;
        }

        DOTween.Sequence().AppendInterval(0.25f).AppendCallback(() =>
        {
            UIClear.Show();
        });
    }

    public void ShowConfimPopupMiniGame()
    {
        popupMiniGame.Show();
    }

}