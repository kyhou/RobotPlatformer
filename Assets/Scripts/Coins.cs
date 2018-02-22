using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Coins : MonoBehaviour
{
    public void OnTriggerEnter2D()
    {
        GameManager.Instance.AddCoins(1);
        Destroy(this.gameObject);
    }
}
