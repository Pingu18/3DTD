using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    private BuildController buildController;    // reference to BuildController script
    private Rigidbody rb;                       // reference to Rigidbody component
    private BasicAttack basicAttack;            // reference to BasicAttack script
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
    private bool readyToJump;
    private float jumpCooldown;

    [Header("Animations")]
    [SerializeField] private Animator playerAnim; // animation controller for the player

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

    private void Start()
    {
        Debug.Log("Getting BuildController script... (PlayerController)");
        buildController = buildControllerObj.GetComponent<BuildController>();

        Debug.Log("Getting components... (PlayerController)");
        rb = GetComponent<Rigidbody>();

        teleport = GetComponent<Teleport>();

        basicAttack = GetComponent<BasicAttack>();
        playerObj = GetComponent<PlayerObject>();

        distanceToGround = model.GetComponent<CapsuleCollider>().bounds.extents.y;
        readyToJump = true;
        jumpCooldown = 0.25f;
    }
    private void Update()
    {
        if (!buildController.getInBuild())
        {
            input();
            controlDrag();
            jump();

            startBasicAttack();
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);

        if (Input.GetKey(KeyCode.Space) && isGrounded && readyToJump)
        {
            readyToJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump");
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private void startBasicAttack()
    {
        if (Input.GetMouseButtonDown(0) && !teleport.inTeleport)
        {
            Debug.Log("Attacking...");

            int rand = Random.Range(0, 2);
            
            if (rand == 0)
                playerAnim.SetTrigger("Attack1");
            else
                playerAnim.SetTrigger("Attack2");

            if (element.Equals("Fire"))
                StartCoroutine(basicAttack.spawnVFX(0.4f));
                //basicAttack.spawnVFX();

            // Sample logic for how to reduce enemy hp on hit by tower (in this case, when shot by player)
            /*
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
            */
        }
    }

    /*
    private void testSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > primarySkillCDTimer)
        {
            primarySkillCDTimer = Time.time + primarySkillCD;
            StartCoroutine(timerUI.startCooldown(1));
            //timerUI.startCooldown(1);
            GameObject skill = Instantiate(primarySkill, this.transform.position, Quaternion.identity);
            skill.transform.parent = this.transform;
            StartCoroutine(PlaySkill1(skill));
        }
    }
    */
    
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

    /*
    IEnumerator PlaySkill1(GameObject skill)
    {
        playerAnim.SetTrigger("Skill1");
        yield return new WaitForSeconds(0.5f);
        skill.transform.GetChild(0).GetComponent<VisualEffect>().Play();
        skill.GetComponent<SphereCollider>().enabled = true;
        Destroy(skill, 1f);
    }
    */
}
