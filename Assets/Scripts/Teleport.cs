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
    [SerializeField] private BuildController buildCon;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator fxAnim;
    private AudioController audioCon;
    private Flamethrower flamethrower;

    private bool canTeleport;
    private RaycastHit hitInfo;
    public bool inTeleport;
    public float teleportCD = 3f;
    public float teleportTimer = 0.0f;
    private Vector3 tpLocation;

    private void Start()
    {
        inTeleport = false;
        flamethrower = FindObjectOfType<Flamethrower>();
        audioCon = FindObjectOfType<AudioController>();
    }

    private void Update()
    {
        if (inTeleport && Time.time > teleportTimer)
        {
            flamethrower.setCanUseFlamethrower(false);
            checkCanTeleport();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canTeleport && !buildCon.getInBuild())
            {
                playerAnim.SetTrigger("ActivateTP");
                fxAnim.SetTrigger("ActivateTP");
                tpLocation = new Vector3(hitInfo.point.x, hitInfo.point.y + 1f, hitInfo.point.z);
                Invoke(nameof(TP), 0.35f);
            }
        } else
        {
            flamethrower.setCanUseFlamethrower(true);
            teleportIndicator.SetActive(false);
            canTeleport = false;
        }
    }

    private void checkCanTeleport()
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitInfo, 200f, (1 << LayerMask.NameToLayer("Default") | (1 << LayerMask.NameToLayer("Placeable"))), QueryTriggerInteraction.Ignore)) {
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
            playerAnim.ResetTrigger("ExitTP");
            playerAnim.SetTrigger("EnterTP");
            fxAnim.SetTrigger("EnterTP");
        } else if (inTeleport)
        {
            inTeleport = false;
            playerAnim.SetTrigger("ExitTP");
            fxAnim.SetTrigger("ExitTP");
        }
    }

    private void TP()
    {
        audioCon.PlaySound("Teleport", player);
        player.transform.position = tpLocation;
        teleportTimer = Time.time + teleportCD;
        StartCoroutine(timerUI.startCooldown(4));
        inTeleport = false;
    }
}
