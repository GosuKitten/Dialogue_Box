using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    [Header("Raycast Offsets")]
    public Vector3 upOffset;
    public Vector3 downOffset;
    public Vector3 leftOffset;
    public Vector3 rightOffset;
    public LayerMask colMask;

    [Range(2, 10)]
    public int rayCount;
    public float edgePadding;

    public float random;
    public string[] wallGroundTags;

    [HideInInspector]
    public bool upCol = false;
    [HideInInspector]
    public bool downCol = false;
    [HideInInspector]
    public bool leftCol = false;
    [HideInInspector]
    public bool rightCol = false;
    [HideInInspector]
    public bool hitEnemy = false;

    [HideInInspector]
    public Vector3 upColPoint;
    [HideInInspector]
    public Vector3 downColPoint;
    [HideInInspector]
    public Vector3 leftColPoint;
    [HideInInspector]
    public Vector3 rightColPoint;

    [HideInInspector]
    public Color lastGroundColor;
    [HideInInspector]
    public Color lastCeilingColor;

    bool canMoveHor;

    // needed variables
    Vector3 velocity;

    public void CalculateCollision(Vector3 _velocity)
    {
        velocity = _velocity;

        canMoveHor = true;
        hitEnemy = false;
        ProcessLeftRaycasts();
        ProcessRightRaycasts();
        ProcessUpRaycasts();
        ProcessDownRaycasts();
    }
    void ProcessLeftRaycasts()
    {
        if (velocity.x < 0)
        {
            RaycastHit2D[] rayHits = new RaycastHit2D[rayCount];
            Vector3 sideOffset = transform.position + leftOffset;
            Vector3 padding = new Vector3(0, edgePadding);
            Vector3 edgePos = downOffset + padding;
            Vector3 endPos = upOffset - padding;
            Vector3 rayDelta = (endPos - edgePos) / (rayCount - 1);
            Vector3 dir = Vector3.left;
            float distance = Mathf.Abs(velocity.x * Time.deltaTime);

            leftCol = false;
            leftColPoint = Vector3.zero;

            for (int i = 0; i < rayCount; i++)
            {
                RaycastHit2D currentRay = rayHits[i] = Physics2D.Raycast(sideOffset + edgePos + (rayDelta * i), dir, distance, colMask);
                if (currentRay)
                {
                    bool hitWallGround = false;
                    foreach (string tag in wallGroundTags)
                    {
                        if (currentRay.transform.tag == tag) hitWallGround = true;
                        if (currentRay.transform.tag == "Enemy") hitEnemy = true;
                    }

                    if (hitWallGround)
                    {
                        leftCol = true;
                        leftColPoint = currentRay.point;
                        canMoveHor = false;
                    }
                }
            }
        }
        else
        {
            leftCol = false;
            leftColPoint = Vector3.zero;
        }
    }

    void ProcessRightRaycasts()
    {
        if (velocity.x > 0)
        {
            RaycastHit2D[] rayHits = new RaycastHit2D[rayCount];
            Vector3 sideOffset = transform.position + rightOffset;
            Vector3 padding = new Vector3(0, edgePadding);
            Vector3 edgePos = downOffset + padding;
            Vector3 endPos = upOffset - padding;
            Vector3 rayDelta = (endPos - edgePos) / (rayCount - 1);
            Vector3 dir = Vector3.right;
            float distance = Mathf.Abs(velocity.x * Time.deltaTime);

            rightCol = false;
            rightColPoint = Vector3.zero;

            for (int i = 0; i < rayCount; i++)
            {
                RaycastHit2D currentRay = rayHits[i] = Physics2D.Raycast(sideOffset + edgePos + (rayDelta * i), dir, distance, colMask);
                if (currentRay)
                {
                    bool hitWallGround = false;
                    foreach (string tag in wallGroundTags)
                    {
                        if (currentRay.transform.tag == tag) hitWallGround = true;
                        if (currentRay.transform.tag == "Enemy") hitEnemy = true;
                    }

                    if (hitWallGround)
                    {
                        rightCol = true;
                        rightColPoint = currentRay.point;
                        canMoveHor = false;
                    }
                }
            }
        }
        else
        {
            rightCol = false;
            rightColPoint = Vector3.zero;
        }
    }

    void ProcessDownRaycasts()
    {
        // down raycast
        if (velocity.y <= 0)
        {
            RaycastHit2D[] rayHits = new RaycastHit2D[rayCount];
            Vector3 sideOffset = transform.position + downOffset;
            Vector3 padding = new Vector3(edgePadding, 0);
            Vector3 edgePos = leftOffset + padding;
            Vector3 endPos = rightOffset - padding;

            if (canMoveHor)
            {
                edgePos += new Vector3(velocity.x * Time.deltaTime, 0);
                endPos += new Vector3(velocity.x * Time.deltaTime, 0);
            }

            Vector3 rayDelta = (endPos - edgePos) / (rayCount - 1);
            Vector3 dir = Vector3.down;
            float distance = Mathf.Abs(velocity.y * Time.deltaTime);

            downCol = false;
            downColPoint = Vector3.zero;

            for (int i = 0; i < rayCount; i++)
            {
                RaycastHit2D currentRay = rayHits[i] = Physics2D.Raycast(sideOffset + edgePos + (rayDelta * i), dir, distance, colMask);
                if (currentRay)
                {
                    bool hitWallGround = false;
                    foreach (string tag in wallGroundTags)
                    {
                        if (currentRay.transform.tag == tag) hitWallGround = true;
                        if (currentRay.transform.tag == "Enemy") hitEnemy = true;
                    }

                    if (hitWallGround)
                    {
                        downCol = true;
                        downColPoint = currentRay.point;
                        lastGroundColor = currentRay.transform.GetComponent<SpriteRenderer>().color;
                    }
                }
            }
        }
        else
        {
            downCol = false;
            downColPoint = Vector3.zero;
        }
    }

    void ProcessUpRaycasts()
    {
        if (velocity.y > 0)
        {
            RaycastHit2D[] rayHits = new RaycastHit2D[rayCount];
            Vector3 sideOffset = transform.position + upOffset;
            Vector3 padding = new Vector3(edgePadding, 0);
            Vector3 edgePos = leftOffset + padding;
            Vector3 endPos = rightOffset - padding;

            if (canMoveHor)
            {
                edgePos += new Vector3(velocity.x * Time.deltaTime, 0);
                endPos += new Vector3(velocity.x * Time.deltaTime, 0);
            }

            Vector3 rayDelta = (endPos - edgePos) / (rayCount - 1);
            Vector3 dir = Vector3.up;
            float distance = Mathf.Abs(velocity.y * Time.deltaTime);

            upCol = false;
            upColPoint = Vector3.zero;

            for (int i = 0; i < rayCount; i++)
            {
                RaycastHit2D currentRay = rayHits[i] = Physics2D.Raycast(sideOffset + edgePos + (rayDelta * i), dir, distance, colMask);
                if (currentRay)
                {
                    bool hitWallGround = false;
                    foreach (string tag in wallGroundTags)
                    {
                        if (currentRay.transform.tag == tag) hitWallGround = true;
                        if (currentRay.transform.tag == "Enemy") hitEnemy = true;
                    }

                    if (hitWallGround)
                    {
                        upCol = true;
                        upColPoint = currentRay.point;
                        lastCeilingColor = currentRay.transform.GetComponent<SpriteRenderer>().color;
                    }
                }
            }
        }
        else
        {
            upCol = false;
            upColPoint = Vector3.zero;
        }
    }
}