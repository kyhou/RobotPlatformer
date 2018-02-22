using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    private Transform itemHold;
    private RaycastHit2D hit;
    private Rigidbody2D itemRig;
    private Vector2 releaseVelocity;

    private float distance = 1f;
    private bool holdingItem = false;
    bool buttonPress;

    // Use this for initialization
    void Start()
    {
        itemHold = transform.Find("ItemHold").transform;
        GetComponent<Platformer2DUserControl>().Grab += PickItem;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (buttonPress)
        {
            Physics2D.queriesStartInColliders = false;
            hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
            if (hit.collider != null && hit.collider.tag == "PickUpItem")
            {
                itemRig = hit.collider.GetComponent<Rigidbody2D>();
                holdingItem = true;
            }
        }
        else
        {
            holdingItem = false;
        }*/
        if (!holdingItem)
        {
            Physics2D.queriesStartInColliders = false;
            hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
            if (hit.collider != null && hit.collider.tag == "PickUpItem")
            {
                itemRig = hit.collider.GetComponent<Rigidbody2D>();
                holdingItem = true;
            }
        }

        if (!buttonPress)
        {
            holdingItem = false;
        }


    }

    void PickItem(bool button)
    {
        buttonPress = button;
        if (holdingItem)
        {
            //itemRig.bodyType = RigidbodyType2D.Static;
            itemRig.simulated = false;
            itemRig.GetComponent<Collider2D>().transform.SetParent(itemHold);
            itemRig.transform.localPosition = Vector2.zero;
            itemRig.velocity = Vector2.zero;
        }
        else
        {
            if (itemRig != null)
            {
                itemRig.simulated = true;
                //itemRig.bodyType = RigidbodyType2D.Dynamic;
            }

            if (itemHold.childCount != 0)
            {
                if(Input.GetAxis("Vertical") > 0)
                {
                    releaseVelocity = new Vector2(0f, 20f);
                }
                else if(Input.GetAxis("Vertical") < 0)
                {
                    releaseVelocity = new Vector2(0f, 0f);
                }
                else
                {
                    releaseVelocity = new Vector2(15f * GetComponentInParent<PlatformerCharacter2D>().transform.localScale.x, 0f);
                }

                itemRig.AddForce(releaseVelocity,ForceMode2D.Impulse);

                itemHold.DetachChildren();
            }
        }
    }
}