//  StringUtils.cs
//  ProductName EraserCrashers
//
//  Created by kikuchikan on 2015.07.31.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 文字列関係の便利クラス
/// </summary>
public static class StringUtility {

	/// <summary>
	/// 指定した文字数のランダムな文字列を返す
	/// </summary>
	public static string GenerateRandomString(int length){
		var sb  = new System.Text.StringBuilder( length );
		var r   = new System.Random();

		string alphabetChars = "abcdefghijklmnopqrstuvwxyz";
		for (int i = 0; i < length; i++){
			int     pos = r.Next( alphabetChars.Length );
			char    c   = alphabetChars[ pos ];
			sb.Append( c );
		}

		return sb.ToString();
	}

}