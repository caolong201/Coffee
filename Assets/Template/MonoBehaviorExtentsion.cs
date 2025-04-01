//  MonoBehaviorExtentsion.cs
//  
//
//  Created by kan.kikuchi

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MonoBehaviorの拡張クラス
/// </summary>
public static class MonoBehaviorExtentsion
{

	public static IEnumerator DelayMethod<T1, T2, T3, T4>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
	{
		yield return new WaitForSeconds(waitTime);
		action(t1, t2, t3, t4);
	}
	/// <summary>
	/// 渡されたメソッドを指定時間後に実行する
	/// </summary>
	public static IEnumerator DelayMethod<T1, T2, T3>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
	{
		yield return new WaitForSeconds(waitTime);
		action(t1, t2, t3);
	}

	/// <summary>
	/// 渡されたメソッドを指定時間後に実行する
	/// </summary>
	public static IEnumerator DelayMethod<T1, T2>(this MonoBehaviour mono, float waitTime, Action<T1, T2> action, T1 t1, T2 t2)
	{
		yield return new WaitForSeconds(waitTime);
		action(t1, t2);
	}

	/// <summary>
	/// 渡されたメソッドを指定時間後に実行する
	/// </summary>
	public static IEnumerator DelayMethod<T>(this MonoBehaviour mono, float waitTime, Action<T> action, T t)
	{
		yield return new WaitForSeconds(waitTime);
		action(t);
	}

	/// <summary>
	/// 渡されたメソッドを指定時間後に実行する
	/// </summary>
	public static IEnumerator DelayMethod(this MonoBehaviour mono, float waitTime, Action action)
	{
		yield return new WaitForSeconds(waitTime);
		action();
	}

}