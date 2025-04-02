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


    float currentDay;
    public void NextLevelButton()
    {
        Debug.Log("Next Level Clicked. Current Day: " + currentDay);

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
        currentDay = day;
        dayText.text = "Day " + day.ToString();

        Debug.Log("Mini game:ParticalProgress:Complete: " + day);
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
