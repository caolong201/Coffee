using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopupOver : MonoBehaviour
{
    [SerializeField] private Transform root;

    public void Show()
    {
        gameObject.SetActive(true);
        root.localScale = Vector3.zero;
        root.DOScale(new Vector3(0.35f, 0.35f, 1), 0.3f).SetEase(Ease.OutBounce);
    }

    public void OnbtnContinueClicked()
    {
        Hide();
        Debug.Log("LevelIndex: " + UserData.LevelIndex);
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