using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSave
{
    public int coins = 0;

    public DataSave(GameManager gm)
    {
        coins = gm.GetCoins();
    }

    public DataSave()
    {
        coins = 0;
    }

    public DataSave(DataSave dataSave)
    {
        coins = dataSave.coins;
    }
}
