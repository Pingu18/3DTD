using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("BuildController GameObject Ref")]
    [SerializeField] private GameObject buildControllerObj; // reference to GameObject buildController

    [Header("Player Model")]
    [SerializeField] private GameObject model;   // reference to GameObject model

    [Header("Player Camera Container Transform")]
    [SerializeField] private Transform pCamTransform;   // reference to player camera container transform

    [Header("Player Orientation")]
    [SerializeField] private Transform orientation;     // reference to player orientation

    [Header("Player Speed")]
    [SerializeField] private float speed;      // player speed

    [Header("Jump Power")]
    [SerializeField] private float jumpForce;  // player jump height

    [Header("Animations")]
    [SerializeField] private Animator playerAnim; // animation controller for the player

    private BuildController buildController;    // reference to BuildController script
    private Rigidbody rb;                       // reference to Rigidbody component

    private float horizontalMovement;
    private float verticalMovement;

    private float movementMultiplier = 10f;
    private float airMultiplier = 0.2f;
    [SerializeField] private float fallMultiplier = 2f;
    private float groundDrag = 6f;
    private float airDrag = 2f;

    private bool isGrounded;        // whether player is currently grounded
    private float distanceToGround; // player's current distance to the ground

    private Vector3 moveDirection;

    private void Start()
    {
        Debug.Log("Getting BuildController script... (PlayerController)");
        buildController = buildControllerObj.GetComponent<BuildController>();

        Debug.Log("Getting components... (PlayerController)");
        rb = GetComponent<Rigidbody>();

        distanceToGround = model.GetComponent<CapsuleCollider>().bounds.extents.y;
    }
    private void Update()
    {
        if (!buildController.getInBuild())
        {
            input();
            controlDrag();
            jump();
        }
        testShooting();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void controlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void input()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void move()
    {
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Acceleration);
        else
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
    }

    private void jump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump");
        }
    }

    private void testShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAnim.SetTrigger("Attack");
            // Sample logic for how to reduce enemy hp on hit by tower (in this case, when shot by player)
            RaycastHit target;
            if (Physics.Raycast(pCamTransform.transform.position, pCamTransform.transform.forward, out target, Mathf.Infinity))
            {
                TowerObject towerObj;

                if (target.collider.GetComponent<TowerObject>())
                {
                    towerObj = target.collider.GetComponent<TowerObject>();
                    Debug.Log(towerObj.getCurrentHP());
                }

                //if (damageable != null)
                    //damageable.takeDamage(100);
            }
        }
    }
    
    // currently unused... might do something else to make player fall faster
    private void fall()
    {
        if (!isGrounded && rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
    }
}
