using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBounce : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;
    private RaycastHit2D hit;
    private Rigidbody2D rig;
    private float distance = 1f;
    private Transform rightCheck;
    private Transform leftCheck;
    private Vector2 boxSize;
    private float checkDepth = 0.1f;
    private Collider2D[] leftColliders;
    private Collider2D[] rightColliders;
    private bool rightMoving;
    private bool leftMoving;
    private bool moving;

    // Use this for initialization
    void Start()
    {
        //mask = LayerMask.GetMask("Ground");
        rig = GetComponent<Rigidbody2D>();
        rightCheck = transform.Find("RightCheck").transform;
        leftCheck = transform.Find("LeftCheck").transform;
        boxSize = new Vector2(checkDepth, GetComponent<BoxCollider2D>().size.y - .05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rig.velocity.x > 0)
        {
            rightMoving = true;
            leftMoving = !rightMoving;
        }
        else if (rig.velocity.x < 0)
        {
            rightMoving = false;
            leftMoving = !rightMoving;
        }
        else
        {
            rightMoving = false;
            leftMoving = false;
        }

        StartCoroutine(LeftCheck());
        StartCoroutine(RightCheck());
    }

    IEnumerator LeftCheck()
    {
        leftColliders = Physics2D.OverlapBoxAll(leftCheck.position, boxSize, 0f, mask);

        for (int i = 0; i < leftColliders.Length; i++)
        {
            if (leftColliders[i].gameObject != gameObject)
            {
                if (leftColliders[i].gameObject.tag == "Player")
                {
                    if (leftMoving)
                    {
                        leftColliders[i].GetComponent<PlatformerCharacter2D>().TakeDamage(false);
                        yield return null;
                    }
                }
                else
                {
                    ChangeDirection();
                    yield return null;
                }
            }
        }
        yield return null;
    }

    IEnumerator RightCheck()
    {
        rightColliders = Physics2D.OverlapBoxAll(rightCheck.position, boxSize, 0f, mask);

        for (int i = 0; i < rightColliders.Length; i++)
        {
            if (rightColliders[i].gameObject != gameObject)
            {

                if (rightColliders[i].gameObject.tag == "Player")
                {
                    if (rightMoving)
                    {
                        rightColliders[i].GetComponent<PlatformerCharacter2D>().TakeDamage(false);
                        yield return null;
                    }
                }
                else
                {
                    ChangeDirection();
                    yield return null;
                }
            }
        }
        yield return null;
    }

    private void ChangeDirection()
    {
        rig.velocity = new Vector2(rig.velocity.x * -1, rig.velocity.y);
    }

    private void OnDrawGizmos()
    {
        if (rightCheck != null)
            Gizmos.DrawCube(rightCheck.position, boxSize);
        if (leftCheck != null)
            Gizmos.DrawCube(leftCheck.position, boxSize);
    }
}