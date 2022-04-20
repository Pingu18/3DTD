using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;     // reference to Rigidbody component

    [SerializeField] private float speed;      // player speed
    [SerializeField] private float jumpForce;  // player jump height

    private bool isGrounded;        // whether player is currently grounded
    private float distanceToGround; // player's current distance to the ground

    void Start()
    {
        Debug.Log("Getting components... (PlayerController)");
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }
    void Update()
    {
        // Jump logic for player
        jump();
    }

    private void FixedUpdate()
    {
        // Movement logic for player
        move();
    }

    public void move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis) * Time.fixedDeltaTime * speed;
        Vector3 newPos = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPos);
    }

    private void jump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }
}
