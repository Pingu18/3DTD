using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private GameObject buildControllerObj; // reference to GameObject buildController
    [SerializeField] private GameObject playerUI; // reference to crosshair container 

    [Header("Camera References")]
    [SerializeField] private GameObject playerCamRef;   // reference to GameObject player camera
    [SerializeField] private GameObject buildCamRef;    // reference to GameObject build camera

    [Header("Transform References")]
    [SerializeField] private Transform playerCamTransform;  // reference to player camera container transform
    [SerializeField] private Transform orientation;         // reference to player orientation

    [Header("Sensitivity")]
    [SerializeField] private float sensX = 10f;     // x sensitivity
    [SerializeField] private float sensY = 10f;     // y sensitivity

    private BuildController buildController;    // reference to BuildController script
    private MenuUI menuUI;

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
        Debug.Log("Getting BuildController script... (CameraController)");
        buildController = buildControllerObj.GetComponent<BuildController>();
        menuUI = FindObjectOfType<MenuUI>();

        Debug.Log("Getting cameras... (CameraController)");
        pCam = playerCamRef.GetComponent<Camera>();
        pCam.enabled = !buildController.getInBuild();

        bCam = buildCamRef.GetComponent<Camera>();
        bCam.enabled = buildController.getInBuild();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!buildController.getInBuild() && !menuUI.getInMenu())
            rotateCamera();
    }

    public void setSens(float sens)
    {
        sensX = sens;
        sensY = sens;
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
        playerUI.SetActive(!buildController.getInBuild());
    }

    public void toggleCamera()
    {
        pCam.enabled = !buildController.getInBuild();
        bCam.enabled = buildController.getInBuild();
    }

    public Camera getActiveCamera()
    {
        if (pCam.enabled)
            return pCam;
        else
            return bCam;
    }

    private void rotateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * 10 * multiplier;
        xRotation -= mouseY * sensY * 10 * multiplier;

        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        playerCamTransform.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // Setter methods
    public void setSensitivity(float sx, float sy)
    {
        sensX = sx;
        sensY = sy;
    }
}
