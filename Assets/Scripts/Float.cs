using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float distance;
    public float floatSpeed;

    private float minDistance;
    private float maxDistance;
    private float currentY;

    private bool moveDown = true;

    private void Start()
    {
        minDistance = transform.localPosition.y - distance;
        maxDistance = transform.localPosition.y;
        currentY = transform.localPosition.y;
    }

    private void FixedUpdate()
    {
        if (moveDown)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y - floatSpeed, 0);
            currentY -= floatSpeed;
        } else
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y + floatSpeed, 0);
            currentY += floatSpeed;
        }

        if (currentY >= maxDistance)
            moveDown = true;
        else if (currentY <= minDistance)
            moveDown = false;
    }
}
