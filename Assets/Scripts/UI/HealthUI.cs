using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private CameraController cameraController;
    private Camera activeCamera;

    private void Start()
    {
        Debug.Log("Getting cameraController... (HealthUI)");
        cameraController = GameObject.Find("PlayerContainer").GetComponent<CameraController>();

        activeCamera = cameraController.getActiveCamera();
    }

    private void FixedUpdate()
    {
        lookAtCamera();
    }

    private void lookAtCamera()
    {
        activeCamera = cameraController.getActiveCamera();
        transform.LookAt(transform.position + activeCamera.transform.rotation * Vector3.forward, activeCamera.transform.rotation * Vector3.up);
    }
}
