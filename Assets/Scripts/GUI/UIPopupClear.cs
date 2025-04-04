using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopupClear : MonoBehaviour
{
    [SerializeField] private Transform root;

    public void Show()
    {
        gameObject.SetActive(true);
        root.localScale = Vector3.zero;
        root.DOScale(new Vector3(0.35f, 0.35f, 1), 0.3f).SetEase(Ease.OutBounce);
        
        if (UserData.LevelIndex <= 1)
        {
            GameAnalyticsManager.Instance.TrackEvent(string.Format("Stage1:WholeProgress:Tutorial:Complete"));
        }
        else
        {
            GameAnalyticsManager.Instance.TrackEvent(string.Format("Stage{0}:WholeProgress:Level:Complete",UserData.LevelIndex));
        }
    }

    public void OnbtnContinueClicked()
    {
        Hide();
        UserData.IsPlayingMiniGame = false;
      
        if (UserData.LevelIndex > 5)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) SceneManager.LoadScene("Game");
            else UIController.Instance.ShowConfimPopupMiniGame();
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}