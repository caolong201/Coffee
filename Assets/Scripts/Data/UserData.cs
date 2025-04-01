using System;
using System.Collections.Generic;


using UnityEngine;

namespace Data
{
    [Serializable]
    public class UserDataV1 : SavePlayerPrefs
    {
        public int day;
        public float coin;
        public List<int> unlockedFoodIdList = new List<int>();
    }

   
}