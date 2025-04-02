using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopupMiniGame : MonoBehaviour
{
    [SerializeField] private Transform root;
    GameAnalyticsManager gameAnalyticsManager;

    public void Show()
    {
        gameObject.SetActive(true);
        root.localScale = Vector3.zero;
        root.DOScale(new Vector3(0.35f, 0.35f, 1), 0.3f).SetEase(Ease.OutBounce);
    }

    public void OnbtnPlayClicked()
    {
        Hide();
        UserData.IsPlayingMiniGame = true;
        SceneManager.LoadScene("MiniGame");
    }

    public void OnbtnSkipClicked()
    {
        int numberSkip = PlayerPrefs.GetInt("numberSkip", 0);
        numberSkip++;
        PlayerPrefs.SetInt("numberSkip", numberSkip);
        GameAnalyticsManager.Instance.TrackEvent(string.Format("Mini game:ParticalProgress:SKIP:{0}", numberSkip));
        PlayerPrefs.Save(); 

        Hide();
        SceneManager.LoadScene("Game");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}