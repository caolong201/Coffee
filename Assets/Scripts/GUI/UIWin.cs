using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWin : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI visitorText;

    [SerializeField] Button nextLevelButton;

    public void NextLevelButton()
    {
        Hide();
        SceneManager.LoadScene("Game");
        //GameManager.Instance.ChangeState(GameStates.NextLevel);
    }

    public void SetCoinText(float coin)
    {
       // coinText.text = coin.ToString();
       UserData.TotalMoneyInGame += (int)coin;
    }

    public void SetDayText(float day)
    {
        dayText.text = "Day " + day.ToString();
    }

    public void SetVisitorText(float v)
    {
        visitorText.text = v.ToString();
    }

    private void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevelButton);
    }

}
