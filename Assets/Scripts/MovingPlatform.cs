using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float distanceX = 0;
    [SerializeField]
    private float distanceY = 0;

    private Vector2 initialPosition;
    private Vector2 newPosition;
    private Vector2 tempPosition;
    private Vector2 boxSize;

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        newPosition = new Vector2(transform.position.x + distanceX, transform.position.y + distanceY);
        tempPosition = newPosition;

        boxSize = GetComponent<SpriteRenderer>().size;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceX != 0 || distanceY != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, tempPosition, speed * Time.deltaTime);

            if ((Vector2)transform.position == tempPosition)
            {
                tempPosition = tempPosition != initialPosition ? initialPosition : newPosition;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {        
        Vector3 contactPoint = col.contacts[0].point;
        Vector3 center = col.collider.bounds.center;

        if (contactPoint.y < center.y)
        {
            col.transform.SetParent(this.transform);
        }
        
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        transform.DetachChildren();
    }
}