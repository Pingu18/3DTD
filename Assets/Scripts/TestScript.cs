using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private Vector3 targetPos = new Vector3(0, 0, 30);
    private float duration = 1f;
    private float elapsedTime;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float percentageComplete = elapsedTime / duration;
  
        transform.position = Vector3.Lerp(startPos, targetPos, percentageComplete);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            startPos = transform.position;
            targetPos = new Vector3(20, 20, 0);
            elapsedTime = 0f;
        }
  
    }

    private void AttackTarget(Vector3 targetPosition)
    {
        // travel from current object to target

    }
}
