using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DayManager : Singleton<DayManager>, IOnStart
{
    public DayConfig dayConfig;
    [SerializeField] FoodConfig foodConfig;
    [SerializeField] FoodObjectPool foodObjectPool;
    public FoodObjectPool FoodObjectPool => foodObjectPool;
    [SerializeField] PassengerManager passengerManager;
    [SerializeField] List<FoodSpot> foodSpots;
    [SerializeField] Transform spawnFoodPoint;
    [SerializeField] Transform despawnFoodPoint;

    //for Debuging !!
    [SerializeField] bool useDayInUserData = false;
    public bool UseDayInUserData => useDayInUserData;

    [SerializeField] int dayIndex = 0;
    public int DayIndex => dayIndex;

    public int MaxDay { get; set; }

    public Dictionary<int, Food> FoodDict = new Dictionary<int, Food>();
    public Dictionary<int, Day> DayDict = new Dictionary<int, Day>();
    List<FoodController> currentFoodControllers = new List<FoodController>();

    public bool WaveFinished { get; set; } = true;
    bool isFinishAllWaves = false;
    public bool IsLastWave { get; set; } = false;   
    public bool NeedTutorial = false;
    float waveCoin = 0;
    public float DayCoin { get; private set; } = 0;
    public float TotalDayCoin { get; private set; } = 0;
    public int TotalDayPassenger { get; private set; } = 0;
    public int ServedPassenger { get; private set; } = 0;

    Coroutine spawnFoodCoroutine;
    public void OnStart()
    {
        FoodDict = foodConfig.CreateFoodDictionary();
        DayDict = dayConfig.CreateDayDictionary();
        MaxDay = DayDict.Values.Count;

        if (useDayInUserData)
        {
            dayIndex = GameManager.Instance.UserData.day;
        }
    }

    public void CleanPassengers()
    {
       // passengerManager.NextPassengerController.Refresh();
    }

    public bool HasShop() => DayIndex >= DayManager.Instance.dayConfig.licenceDay;

    public void Resume()
    {
        VFXAnimationManager.Instance.StopAngryEmoji();
        GameUI.Instance.Get<UIInGame>().Show();
        GameUI.Instance.Get<UIInGame>().Reset();
    }

    public void Clear(bool isRetry)
    {
        Reset(isRetry);
        // passengerManager.CurrentPassengerController.Reset();
        // passengerManager.NextPassengerController.Reset(isRetry);
    }

    public void LoadDay(bool isRetry = false)
    {
        GameManager.Instance.SetupIngame();
        var userData = GameManager.Instance.UserData;
        if (UseDayInUserData)
        {
            while (userData.day >= MaxDay)
            {
                GenerateRandomDay();
            }
        }
        Clear(isRetry);

        GameUI.Instance.Get<UIInGame>().Show();

        if (NeedTutorial)
        {
            if(dayIndex != 0)
            {
                NeedTutorial = false;
            }
            else if(dayIndex == dayConfig.licenceDay)
            {

            }
            else
            {
                GameManager.Instance.ChangeState(GameStates.Tutorial);
            }  
        }
        Day day = DayDict[dayIndex];
        foreach (Passenger passenger in day.PassengerList)
        {
            foreach (FoodOrder foodOrder in passenger.FoodOrderList)
            {
                int id = foodOrder.FoodId;
                int quantity = foodOrder.Quantity;

                Food food = FoodDict[id];
                TotalDayCoin += userData.GetFoodPrice(food) * quantity;
            }
            TotalDayPassenger++;
        }

        GameUI.Instance.Get<UIInGame>().SetProgress(DayCoin, TotalDayCoin);
        GameUI.Instance.Get<UIInGame>().SetDayText(dayIndex);
        GameUI.Instance.Get<UIInGame>().UpdateCoinLb();
        spawnFoodCoroutine = StartCoroutine(SpawnFoodToTable(day));
        passengerManager.GetRandomPassengerObject();
        
    }

    IEnumerator SpawnFoodToTable(Day day)
    {
        for(int i = 0; i < day.PassengerList.Count; i++)
        {
            Passenger passenger = day.PassengerList[i];
           
            yield return new WaitUntil(() => WaveFinished);
            passengerManager.GetRandomPassengerObject();
            if (i == day.PassengerList.Count - 1)
            {
                IsLastWave = true;
            }
            yield return new WaitForSeconds(.25f);
            
            foreach (FoodOrder foodOrder in passenger.FoodOrderList)
            {
                int id = foodOrder.FoodId;
                int quantity = foodOrder.Quantity;

                Food food = FoodDict[id];
                int index = quantity - 1;
             
              
                //Instantiate(food.foodStacks[index], spawnFoodPoint);

                FoodSpot foodSpot = GetFoodSpot();
                if (foodSpot == null)
                {
                    Debug.LogError("Can't have that many food at once !!!");
                    yield break;
                }
                else
                {
                    if (!GameManager.Instance.CheckUnlockedFood(id))
                    {
                        GameUI.Instance.Get<UIUnlockFood>().Show();
                        GameUI.Instance.Get<UIUnlockFood>().AddToShowList(food);
                    }

                    waveCoin += GameManager.Instance.UserData.GetFoodPrice(food) * quantity;
                   
                    FoodController foodController = foodObjectPool.GetFood(id, quantity, food.foodStacks[index].gameObject, spawnFoodPoint);
                    foodController.SetFoodSpot(foodSpot, despawnFoodPoint, id, quantity);
                    currentFoodControllers.Add(foodController);
                    foodSpot.IsHadFood = true;
                }
            }

            WaveFinished = false;
        }

        isFinishAllWaves = true;

    }

    FoodSpot GetFoodSpot()
    {
        foreach(FoodSpot foodSpot in foodSpots)
        {
            if (!foodSpot.IsHadFood)
            {
                return foodSpot;
            }
        }

        return null;
    }

    public void Cheat()
    {
        CheckAnswer(waveCoin);
    }

    public void CheckAnswer(float answer)
    {
        if(answer != waveCoin)
        {
            if (NeedTutorial)
            {
                GameUI.Instance.Get<UIInGame>().SetRedColorValueText();
                GameUI.Instance.Get<UITutorial>().ShowFailedStage();
                return;
            }
            else
            {
                VFXAnimationManager.Instance.PlayAngryEmoji();
                StartCoroutine(EndGameResult(false));
                return;
            } 
        }

        if (NeedTutorial)
        {
            GameUI.Instance.Get<UITutorial>().ShowSuccessStage();
            NeedTutorial = false;
        }

        GameUI.Instance.Get<UIInGame>().SetGreenColorValueText();

        GameManager.Instance.AddCoin(answer);

        foreach (FoodController controller in currentFoodControllers)
        {
            controller.ChangeState(FoodStage.OnBilled);
        }
        

        DayCoin += waveCoin;
        ServedPassenger++;
        GameUI.Instance.Get<UIInGame>().SetProgress(DayCoin, TotalDayCoin);
        waveCoin = 0;

        passengerManager.MoveOut();
        //VFXAnimationManager.Instance.PlayHappyEmoji();
        DOVirtual.DelayedCall(2f,()=>{
            WaveFinished = true;
        });
       
        currentFoodControllers.Clear();


        if (isFinishAllWaves)
        {
            StartCoroutine(EndGameResult(true));
            return;
        }
    }

    void Reset(bool isRetry)
    {
        
        if (isRetry)
        {
            VFXAnimationManager.Instance.StopAngryEmoji();
            foodObjectPool.RemoveAllFood();
            //cheat fix remove all food bug
            foreach(var foodspot in foodSpots)
            {
                foreach(Transform foodTF in foodspot.transform)
                {
                    foodTF.gameObject.SetActive(false);
                }
            }
        }

        if(spawnFoodCoroutine != null) StopCoroutine(spawnFoodCoroutine);
        waveCoin = 0;
        TotalDayCoin = 0;
        DayCoin = 0;
        ServedPassenger = 0;
        WaveFinished = true;
        isFinishAllWaves = false;
        currentFoodControllers.Clear();
        IsLastWave = false;
        TotalDayPassenger = 0;
        dayIndex = GameManager.Instance.UserData.day;

        foreach(FoodSpot foodSpot in foodSpots)
        {
            foodSpot.IsHadFood = false;
        }
    }

    IEnumerator EndGameResult(bool isWon)
    {

        if (isWon)
        {
            yield return new WaitForSeconds(0.75f);
            GameManager.Instance.ChangeState(GameStates.Win);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
            GameManager.Instance.ChangeState(GameStates.Lose);
        }
       
    }

    public void RemoveLast()
    {
        var id = dayConfig.Days.Count - 1;
        dayConfig.Days.RemoveAt(id);
        DayDict.Remove(id);
        MaxDay--;
    }

    public void GenerateRandomDay()
    {
        var curSales = 0;
        Day newDay = new Day();
        newDay.DayID = dayConfig.Days.Count;
        var goalSales = GameHelper.GetGoalSale(newDay.DayID);

        while (curSales < goalSales)
        {
            Passenger newPassenger = new Passenger();

            int orderCount = Random.Range(2, 5);
            for (int k = 0; k < orderCount; k++)
            {
                FoodOrder newOrder = new FoodOrder();

                int randomFoodId = GetRandomFoodId(curSales);

                if (FoodDict.TryGetValue(randomFoodId, out Food selectedFood))
                {
                    int maxQuantity = selectedFood.foodStacks.Count;
                    newOrder.Quantity = new System.Random(Time.frameCount - randomFoodId - curSales).Next(1, maxQuantity + 1);
                    newOrder.FoodId = randomFoodId;
                    curSales += selectedFood.GetPrice(GameManager.Instance.UserData.GetFoodLevel(randomFoodId)) * newOrder.Quantity;
                }

                newPassenger.FoodOrderList.Add(newOrder);
                if (curSales >= goalSales) break;
            }

            newDay.PassengerList.Add(newPassenger);
        }
        dayConfig.Days.Add(newDay);

        DayDict.Add(newDay.DayID, newDay);
        MaxDay++;
    }

    private int GetRandomFoodId(int seed)
    {
        var foodIds = GameManager.Instance.UserData.unlockedFoodIdList;
        int randomIndex = new System.Random(seed + Time.frameCount).Next(foodIds.Count);
        return foodIds[randomIndex];
    }

    public void ReduceDay() => dayIndex--;
}
