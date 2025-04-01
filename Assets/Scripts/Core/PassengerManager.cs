using System.Collections.Generic;
using DG.Tweening;
using TCP_Fatness_Level;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public GuessController Guess;
    
    public void GetRandomPassengerObject()
    {
        Debug.Log("MoveIN");
        Guess.LoadSkinMiniGame();
    }

    public void MoveOut()
    {
        Debug.Log("MoveOut");
        Guess.MoveOut();
    }
}