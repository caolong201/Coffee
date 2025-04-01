//  EnumUtility.cs
//  ProductName Template
//
//  Created by kan.kikuchi on 2016.04.28.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enumの便利クラス
/// </summary>
public static class EnumUtility {

	/// <summary>
	/// 設定された列挙型の全ての値で、入力されたアクションを実行する
	/// </summary>
	public static void ExcuteActionInAllValue<T>(Action<T> action){
		foreach(T t in Enum.GetValues(typeof(T))){
			action (t);
		}
	}

	/// <summary>
	/// 入力されたkeyが設定された列挙型に含まれるか
	/// </summary>
	public static bool ContainsKey<T>(string tagetKey){

		foreach(T t in Enum.GetValues(typeof(T))){
			if(t.ToString() == tagetKey){
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 入力されたkeyと同じ列挙型の項目を取得する
	/// </summary>
	public static T KeyToType<T>(string targetKey){
		return (T)Enum.Parse (typeof(T), targetKey);
	}

	/// <summary>
	/// 入力されたNoと同じ列挙型の項目を取得する
	/// </summary>
	public static T NoToType<T>(int targetNo){
		return (T)Enum.ToObject (typeof(T), targetNo);
	}

	/// <summary>
	/// 指定した列挙型の項目数を取得する
	/// </summary>
	public static int GetTypeNum<T>(){
		return Enum.GetValues (typeof(T)).Length;
	}

}