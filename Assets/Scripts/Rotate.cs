using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;
    private Vector3 rotateVector = new Vector3(0, 0, 1);

    private void Update()
    {
        this.transform.Rotate(rotateVector * (rotateSpeed * Time.deltaTime));
    }
}
