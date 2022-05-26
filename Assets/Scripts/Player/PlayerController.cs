using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    private BuildController buildController;    // reference to BuildController script
    private MenuUI menuUI;
    private Rigidbody rb;                       // reference to Rigidbody component
    private BasicAttack basicAttack;            // reference to BasicAttack script
    [SerializeField] private GameObject secondaryAttack;
    private PlayerObject playerObj;             // reference to PlayerObject script

    private float horizontalMovement;
    private float verticalMovement;

    private float movementMultiplier = 10f;
    private float airMultiplier = 0.2f;
    [SerializeField] private float fallMultiplier = 2f;
    private float groundDrag = 6f;
    private float airDrag = 2f;

    private bool isGrounded;        // whether player is currently grounded
    private float distanceToGround; // player's current distance to the ground
    private int layer_mask;

    private Vector3 moveDirection;

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
    [SerializeField] private float superJumpForce;
    private bool readyToJump;
    private bool readyToSuperJump;
    private float superJumpCooldown;
    private float superJumpTimer = 0.0f;
    private float jumpCooldown;

    [Header("Animations")]
    [SerializeField] private Animator playerAnim; // animation controller for the player
    [SerializeField] private Animator flamethrowerAnim; // animation controller for the flamethrower skill

    [Header("Player Details")]
    [SerializeField] private string element;

    /*
    [Header("Skills")]
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private GameObject primarySkill;
    public float primarySkillCD = 5f;
    public float primarySkillCDTimer = 0.0f;
    */

    private Teleport teleport;
    private float basicAttackCD = 0.75f;
    private float basicAttackTimer = 0.0f;

    private void Start()
    {
        layer_mask = LayerMask.GetMask("Default", "Ground", "Placeable");
        Debug.Log("Getting BuildController script... (PlayerController)");
        buildController = buildControllerObj.GetComponent<BuildController>();
        menuUI = FindObjectOfType<MenuUI>();

        Debug.Log("Getting components... (PlayerController)");
        rb = GetComponent<Rigidbody>();

        teleport = GetComponent<Teleport>();

        basicAttack = GetComponent<BasicAttack>();
        playerObj = GetComponent<PlayerObject>();

        distanceToGround = model.GetComponent<CapsuleCollider>().bounds.extents.y;
        readyToJump = true;
        readyToSuperJump = true;
        jumpCooldown = 0.25f;
        superJumpCooldown = 0.25f;
    }
    private void Update()
    {
        if (!buildController.getInBuild() && !menuUI.getInMenu())
        {
            input();
            controlDrag();
            jump();

            startBasicAttack();
            //startSecondaryAttack();
            //testSkill();
        }
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            teleport.toggleTeleport();
        }
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f, layer_mask);

        if (isGrounded && !readyToSuperJump && Time.time > superJumpTimer)
            readyToSuperJump = true;

        if (Input.GetKey(KeyCode.Space) && isGrounded && readyToJump)
        {
            readyToJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump");
            Invoke(nameof(resetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && readyToSuperJump)
        {
            readyToSuperJump = false;
            superJumpTimer = Time.time + superJumpCooldown;
            rb.AddForce(transform.up * superJumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump");
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private void startBasicAttack()
    {
        if (Input.GetMouseButtonDown(0) && !teleport.inTeleport && Time.time > basicAttackTimer)
        {
            int rand = Random.Range(0, 2);
            
            if (rand == 0)
                playerAnim.SetTrigger("Attack1");
            else
                playerAnim.SetTrigger("Attack2");

            if (element.Equals("Fire"))
                StartCoroutine(basicAttack.spawnVFX(0.4f));
            
            basicAttackTimer = Time.time + basicAttackCD;
        }
    }

    private void startSecondaryAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            secondaryAttack.transform.GetChild(0).GetComponent<VisualEffect>().Play();
            flamethrowerAnim.SetTrigger("StartFlamethrower");
        }

        if (Input.GetMouseButtonUp(1))
        {
            secondaryAttack.transform.GetChild(0).GetComponent<VisualEffect>().Stop();
            flamethrowerAnim.SetTrigger("StopFlamethrower");
        }
    }

    // currently unused... might do something else to make player fall faster
    private void fall()
    {
        if (!isGrounded && rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
    }

    public string getElement()
    {
        return element;
    }

    public BasicAttack getBasicAttack()
    {
        return basicAttack;
    }
}
