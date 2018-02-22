using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic.Extensions;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private TextMeshProUGUI coins;

    // Use this for initialization
    void Start()
    {
        coins = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
    }

    /*// Update is called once per frame
    void Update()
    {
    
    }*/

    public void UpdateCoins() // TEMPORÁRIO, NÃO ESQUECER DE CORRIGIR
    {
        coins.text = "Coins : " + GameManager.Instance.GetCoins().ToString();
    }
}