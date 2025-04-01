//  UserData.cs
//  ProductName Template
//
//  Created by kan.kikuchi on 2016.04.28.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// 永続的に保存するデータを管理するクラス
    /// </summary>
public static class UserData
{
    public static bool NewMode = false;

    public static Action OnNewObject = delegate { };
    public static Action OnMoneyChange = delegate { };

    private const string TOTAL_MONEY_IN_GAME = "TOTAL_MONEY_IN_GAME";

    public static int TotalMoneyInGame
    {
        get
        {
            return PlayerPrefs.GetInt(TOTAL_MONEY_IN_GAME, 0);
        }
        set
        {
            PlayerPrefs.SetInt(TOTAL_MONEY_IN_GAME, value);
            PlayerPrefs.Save();

            OnMoneyChange.Invoke();
        }
    }

    private const string ORDER_INDEX = "ORDER_INDEX";

    public static int OrderIndex
    {
        get
        {
            return PlayerPrefs.GetInt(ORDER_INDEX, 0);
        }
        set
        {
            PlayerPrefs.SetInt(ORDER_INDEX, value);
            PlayerPrefs.Save();
        }
    }

    private const string LEVEL_INDEX = "LEVEL_INDEX";

    public static int LevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt(LEVEL_INDEX, 0);
        }
        set
        {
            PlayerPrefs.SetInt(LEVEL_INDEX, value);
            PlayerPrefs.Save();
        }
    }

    private const string PLAY_COUNT = "PLAY_COUNT";

    public static int PlayCount
    {
        get
        {
            return PlayerPrefs.GetInt(PLAY_COUNT, 0);
        }
        set
        {
            PlayerPrefs.SetInt(PLAY_COUNT, value);
            PlayerPrefs.Save();
        }
    }

    private const string CUSTOMER_LEVEL = "CUSTOMER_LEVEL";

    public static int CustomerLevel
    {
        get
        {
            return PlayerPrefs.GetInt(CUSTOMER_LEVEL, 0);
        }
        set
        {
            PlayerPrefs.SetInt(CUSTOMER_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
    
    private static bool isPlayingMiniGame = false;

    public static bool IsPlayingMiniGame
    {
        get
        {
            return isPlayingMiniGame;
        }
        set
        {
            isPlayingMiniGame = value;
        }
    }
}