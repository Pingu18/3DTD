using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;   // reference to GameObject camera

    private Rigidbody body;     // reference to Rigidbody component

    private float speed = 10f;      // player speed
    private float jumpHeight = 2f;  // player jump height

    private bool isGrounded = true; // whether player is currently grounded
    private float distanceToGround; // player's current distance to the ground

    void Start()
    {
        Debug.Log("Starting PlayerController...");
        body = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<BoxCollider>().bounds.extents.y;
    }
    void Update()
    {
        // Add condition for whether player is in build mode or not
        cameraFollowPlayer();
        rotatePlayer();

        // Movement logic for player
        movement();
    }

    public void movement()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);

        if (Input.GetKey(KeyCode.W))
            body.position += transform.forward * Time.deltaTime * speed;
        else if (Input.GetKey(KeyCode.S))
            body.position -= transform.forward * Time.deltaTime * speed;
        else if (Input.GetKey(KeyCode.A))
            body.position -= transform.right * Time.deltaTime * speed;
        else if (Input.GetKey(KeyCode.D))
            body.position += transform.right * Time.deltaTime * speed;

        if (Input.GetButtonDown("Jump") && isGrounded)
            body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    public void rotatePlayer()
    {
        transform.localEulerAngles = new Vector3(0, camera.transform.localEulerAngles.y, 0);
    }

    public void cameraFollowPlayer()
    {
        camera.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }
}
