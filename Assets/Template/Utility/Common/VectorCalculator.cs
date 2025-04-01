//  VectorCalculator.cs
//  
//
//  Created by kan.kikuchi

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ベクトルの計算を行うクラス
/// </summary>
public static class VectorCalculator {

	/// <summary>
	/// p2からp1への角度を求める
	/// </summary>
	public static float GetAim(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}

	/// <summary>
	/// ある地点から違う地点への向きを求める
	/// </summary>
	public static Vector3 GetDirection(Vector3 start, Vector3 goal) {
		Vector3 heading   = goal - start;
		float   distance  = heading.magnitude;
		Vector3 direction = heading / distance; 

		return direction;
	}

}