using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class GameManager : Singleton<GameManager>
{
    private ObservedValue<int> coins;

    void Start()
    {
        coins = new ObservedValue<int>(0);
        coins.OnValueChange += UpdateUI;
    }

    public void AddCoins(int amount)
    {
        coins.Value += amount;
    }

    public void SubCoins(int amount)
    {
        coins.Value -= amount;
    }

    public int GetCoins()
    {
        return coins.Value;
    }

    public void Save()
    {
        SaveManager.SaveGame();
    }

    public void Load()
    {
        DataSave dataSave = SaveManager.LoadGame(); // new DataSave(SaveManager.LoadGame());
        coins.Value = dataSave.coins;
    }

    public void UpdateUI()
    {
        UIManager.Instance.UpdateCoins();
    }
}