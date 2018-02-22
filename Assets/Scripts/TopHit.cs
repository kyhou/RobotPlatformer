using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHit : MonoBehaviour
{
    private EdgeCollider2D topCollider;

    // Use this for initialization
    void Start()
    {
        topCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    /*private void OnTriggerEnter2D(EdgeCollider2D col)
    {
        
    }*/
}