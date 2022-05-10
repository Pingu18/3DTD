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

    private bool canTeleport;
    private RaycastHit hitInfo;
    public bool inTeleport;
    public float teleportCD = 3f;
    public float teleportTimer = 0.0f;

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
                player.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 1f, hitInfo.point.z);
                teleportTimer = Time.time + teleportCD;
                StartCoroutine(timerUI.startCooldown(4));
                Invoke(nameof(toggleTeleport), 0.05f);
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
        inTeleport = !inTeleport;
    }
}
