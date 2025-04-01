using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyLabel : MonoBehaviour
{
    private TextMeshProUGUI _lbl;

    private void Awake()
    {
        _lbl = GetComponent<TextMeshProUGUI>();

        UpdateData();
    }

    private void OnEnable()
    {
        UserData.OnMoneyChange += UpdateData;
    }

    private void OnDisable()
    {
        UserData.OnMoneyChange += UpdateData;
    }

    // Update is called once per frame
    void UpdateData()
    {
        _lbl.text = UserData.TotalMoneyInGame.ToString();
    }
}
