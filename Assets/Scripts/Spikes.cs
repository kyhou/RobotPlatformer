using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponent<PlatformerCharacter2D>().Spike();
    }
}