using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject buildCamRef;

    private GameObject player;  // reference to GameObject player

    private Camera playerCam;   // reference to player camera
    private Camera buildCam;    // reference to build camera


    [SerializeField] private float sensitivity;    // turn sensitivity
    [SerializeField] private float smoothing;
    [SerializeField] private float x;                   // vertical mouse input
    [SerializeField] private float y;                   // horizontal mouse input
    private Vector2 smoothedVelocity;
    private Vector2 currentLookingPos;

    private float minAngle = -90.0f;    // minimum vertical angle movement
    private float maxAngle = 90.0f;     // maximum verticle angle movement

    private bool inBuild = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Getting player... (CameraController)");
        player = transform.parent.gameObject;

        playerCam = GetComponent<Camera>();
        playerCam.enabled = true;

        buildCam = buildCamRef.GetComponent<Camera>();
        buildCam.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleBuild();
            toggleCamera();
        }

        if (!inBuild)
            rotateCamera();
    }

    public void toggleCamera()
    {
        playerCam.enabled = !playerCam.enabled;
        buildCam.enabled = !buildCam.enabled;
    }

    public void toggleBuild()
    {
        inBuild = !inBuild;
    }

    public void rotateCamera()
    {
        
        // Grab mouse x and y axis movement
        x -= Input.GetAxis("Mouse Y");
        y += Input.GetAxis("Mouse X");

        // Restrict vertical camera movement (so player can't do a full spin vertically)
        x = Mathf.Clamp(x, (minAngle / sensitivity), (maxAngle / sensitivity));

        // Apply rotation around X axis to camera
        transform.localRotation = Quaternion.AngleAxis(x * sensitivity, Vector3.right);

        // Apply rotation around Y axis to player
        player.transform.localRotation = Quaternion.AngleAxis(y * sensitivity, player.transform.up);
        

        /*

        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPos += smoothedVelocity;

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, player.transform.up);
        */
    }

    // Setter methods
    public void setSensitivity(float s)
    {
        sensitivity = s;
    }
}
