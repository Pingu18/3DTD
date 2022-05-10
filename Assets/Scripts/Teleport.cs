using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject teleportIndicator;
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask ground;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private Animator playerAnim;

    private bool canTeleport;
    private RaycastHit hitInfo;
    public bool inTeleport;
    public float teleportCD = 3f;
    public float teleportTimer = 0.0f;
    private Vector3 tpLocation;

    private void Start()
    {
        inTeleport = false;
    }

    private void Update()
    {
        if (inTeleport && Time.time > teleportTimer)
        {
            checkCanTeleport();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canTeleport)
            {
                playerAnim.SetTrigger("ActivateTP");
                tpLocation = new Vector3(hitInfo.point.x, hitInfo.point.y + 1f, hitInfo.point.z);
                Invoke(nameof(TP), 0.35f);
            }
        } else
        {
            teleportIndicator.SetActive(false);
            canTeleport = false;
        }
    }

    private void checkCanTeleport()
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitInfo, 50f)) {
            teleportIndicator.SetActive(true);
            teleportIndicator.transform.position = hitInfo.point;
            canTeleport = true;
        } else
        {
            teleportIndicator.SetActive(false);
            canTeleport = false;
        }
    }

    public void toggleTeleport()
    {
        if (!inTeleport && Time.time >= teleportTimer) // only enter teleport if off cooldown
        {
            inTeleport = true;
            playerAnim.SetTrigger("EnterTP");
        } else if (inTeleport)
        {
            inTeleport = false;
            playerAnim.SetTrigger("ExitTP");
        }
    }

    private void TP()
    {
        player.transform.position = tpLocation;
        teleportTimer = Time.time + teleportCD;
        StartCoroutine(timerUI.startCooldown(4));
        inTeleport = false;
    }
}
