using System;
using System.Collections.Generic;


using UnityEngine;

namespace Data
{
    [Serializable]
    public class UserDataMiniGame : SavePlayerPrefs
    {
        public bool hasUnlockLicense;
        public int day = 0;
        public float coin;
        public List<int> unlockedFoodIdList = new () { 0, 1 };
        public List<int> foodLevels = new() { 0, 0 };
        public List<int> unlockedShopBuildingIds = new();

        public int GetFoodLevel(int foodId)
        {
            return foodLevels.Count > foodId ? foodLevels[foodId] : -1;
        }

        public int GetFoodPrice(Food food)
        {
            return food.GetPrice(GetFoodLevel(food.Id));
        }

        public void UnlockFood(int foodId)
        {
            GameManager.Instance.CheckUnlockedFood(foodId);
            while (foodLevels.Count <= foodId) foodLevels.Add(-1);
            if (foodLevels[foodId] < 0) foodLevels[foodId] = 0;
        }
    }
}