using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("BuildController GameObject Ref")]
    [SerializeField] private GameObject buildControllerObj; // reference to GameObject buildController

    [Header("Player Cam Ref")]
    [SerializeField] private GameObject pCamRef;    // reference to GameObject player camera

    [Header("Build Cam Ref")]
    [SerializeField] private GameObject bCamRef;    // reference to GameObject build camera

    [Header("Player Camera Container Transform")]
    [SerializeField] private Transform pCamTransform;   // reference to player camera container transform

    [Header("Player Orientation")]
    [SerializeField] private Transform orientation;     // reference to player orientation

    [Header("UIContainer Ref")]
    [SerializeField] private GameObject uiContainer;    // reference to UI container 

    [Header("Sensitivity X")]
    [SerializeField] private float sensX = 50f;     // x sensitivity

    [Header("Sensitivity Y")]
    [SerializeField] private float sensY = 50f;     // y sensitivity

    private BuildController buildController;    // reference to BuildController script

    private Camera pCam;    // reference to player camera
    private Camera bCam;    // reference to build camera

    private float xRotation;    // how much to rotate on x axis
    private float yRotation;    // how much to rotate on y axis

    private float minAngle = -90.0f;    // minimum vertical angle movement
    private float maxAngle = 90.0f;     // maximum verticle angle movement
    private float multiplier = 0.01f;   // sensitivity multiplier

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Getting BuildController script... (CameraController");
        buildController = buildControllerObj.GetComponent<BuildController>();

        Debug.Log("Getting cameras... (CameraController)");
        pCam = pCamRef.GetComponent<Camera>();
        pCam.enabled = !buildController.getInBuild();

        bCam = bCamRef.GetComponent<Camera>();
        bCam.enabled = buildController.getInBuild();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!buildController.getInBuild())
            rotateCamera();
    }

    public void toggleMouseLock()
    {
        if (buildController.getInBuild())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void toggleCanvas()
    {
        uiContainer.SetActive(!buildController.getInBuild());
    }

    public void toggleCamera()
    {
        pCam.enabled = !buildController.getInBuild();
        bCam.enabled = buildController.getInBuild();
    }

    private void rotateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        pCamTransform.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // Setter methods
    public void setSensitivity(float sx, float sy)
    {
        sensX = sx;
        sensY = sy;
    }
}
