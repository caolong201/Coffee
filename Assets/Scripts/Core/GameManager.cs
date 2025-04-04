using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using Data;

public class GameManager : Singleton<GameManager>
{
    public Transform furnitureRoot;
    [SerializeField] private GameObject ingameObj;

    public UserDataMiniGame UserData
    {
        get; private set;
    }
    protected override void Awake()
    {
        base.Awake();
        Game.Launch();
        UserData = Game.Data.Load<UserDataMiniGame>();
        GameAnalyticsSDK.GameAnalytics.Initialize();
        AdUtil.Init();
        TrackingUtil.Init();
    }
   

    private void Start()
    {
        //GameUI.Instance.Get<UIStart>().Show();
        DayManager.Instance.OnStart();
        ChangeState(GameStates.Start);

        /*var count = furnitureRoot.childCount;
        for(var i = 0; i < count; i++)
        {
            furnitureRoot.GetChild(i).SetActive(UserData.unlockedFurnitures.Contains(i));
        }*/
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DayManager.Instance.Cheat();
        }
    }


    [ContextMenu("Delete Data")]
    private void DeleteData()
    {
        Debug.LogError("delete data");
        PlayerPrefs.DeleteAll();
    }
#endif

    [SerializeField] private GameStates _state = GameStates.Retry;
    public void ChangeState(GameStates newState)
    {
        if (newState == _state) return;
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {

        switch (_state)
        {
            case GameStates.Tutorial:
                GameUI.Instance.Get<UITutorial>().Show();
                break;
            case GameStates.Home:
                if (UserData.day < DayManager.Instance.dayConfig.licenceDay)
                {
                    ChangeState(GameStates.Start);
                }
                else
                {
                    GameUI.Instance.Get<UIHome>().Show();
                }

                break;
            case GameStates.Start:
                GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"Level:{DayManager.Instance.DayIndex}:Start");
                DayManager.Instance.LoadDay();
                break;
            case GameStates.Play:
              
                break;
            case GameStates.Retry:
                DayManager.Instance.LoadDay(true);
                break;
            case GameStates.Resume:
                DayManager.Instance.Resume();
                break;
            case GameStates.Win:
                GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"Level:{DayManager.Instance.DayIndex}:Complete");
                GameUI.Instance.Get<UIInGame>().Hide();
                GameUI.Instance.Get<UIWin>().Show();
                break;
            case GameStates.Lose:
                //GameUI.Instance.Get<UIInGame>().Hide();
                GameUI.Instance.Get<UILose>().Show();
                break;
            case GameStates.NextLevel:
                DayManager.Instance.LoadDay();
                //ChangeState(GameStates.Home);
                break;
            default:
                break;
        }
    }

    private void ExitCurrentState()
    {
        switch (_state)
        {
            case GameStates.Tutorial:
                break;
            case GameStates.Home:
                break;
            case GameStates.Start:
                break;
            case GameStates.Play:
   
                break;
            case GameStates.Retry:
                break;
            case GameStates.Win:
                break;
            case GameStates.Lose:
                break;
            case GameStates.NextLevel:
          
                break;
            default:
                break;
        }
    }

    public void SetupIngame()
    {
        ingameObj.SetActive(true);
    }

    public void SetupOutgame()
    {
        ingameObj.SetActive(false);
    }

    public bool CheckUnlockedFood(int id)
    {
        if (UserData.unlockedFoodIdList.Contains(id))
        {
           
            return true;
        }
        if (DayManager.Instance.UseDayInUserData)
        {
            UserData.unlockedFoodIdList.Add(id);
            UserData.foodLevels.Add(0);
        }
    
        return false;
    }

    public void AddCoin(float coin)
    {
        UserData.coin += coin;
    }
}

public enum GameStates
{
    None = -1, Play, Win, Lose, Home, Tutorial, Start, Retry, NextLevel, Resume
}
