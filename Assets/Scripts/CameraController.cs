using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;   // reference to GameObject player

    [SerializeField] private float sensitivity = 3f;    // turn sensitivity
    [SerializeField] private float x;                   // vertical mouse input
    [SerializeField] private float y;                   // horizontal mouse input

    private float minAngle = -90.0f;    // minimum vertical angle movement
    private float maxAngle = 90.0f;     // maximum verticle angle movement

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing CameraController...");
    }

    // Update is called once per frame
    void Update()
    {
        mouseLook();
    }

    public void mouseLook()
    {
        // Grab mouse x and y axis movement
        x -= Input.GetAxis("Mouse Y");
        y += Input.GetAxis("Mouse X");

        // Restrict vertical camera movement (so player can't do a full spin vertically)
        x = Mathf.Clamp(x, (minAngle / sensitivity), (maxAngle / sensitivity));

        // Apply to camera
        transform.eulerAngles = new Vector3(x, y, 0) * sensitivity;
    }

    // Setter methods
    public void setSensitivity(float s)
    {
        sensitivity = s;
    }
}
