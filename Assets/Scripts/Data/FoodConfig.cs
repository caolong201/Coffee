using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FoodConfig : ScriptableObject
{
    public List<Food> FoodList = new List<Food>();

    public Dictionary<int, Food> CreateFoodDictionary()
    {
        Dictionary<int, Food> FoodDict = new Dictionary<int, Food>();
        foreach (Food food in FoodList)
        {        
            FoodDict.Add(food.Id, food);          
        }

        return FoodDict;
    }
}

[Serializable]
public class Food
{
    public int Id;
    public string Name;
    public Sprite Icon;
    [SerializeField] private int Price;
    public int levelPrice;
    public int DayUnlock;
    public List<FoodController> foodStacks;
    [SerializeField] private int initialCost;
    [SerializeField] private int levelCost;

    public int GetLicenseCost(int level)
    {
        return initialCost + (level + 1) * levelCost;
    }

    public int GetPrice(int level)
    {
        return Price + level * levelPrice;
    }
}
