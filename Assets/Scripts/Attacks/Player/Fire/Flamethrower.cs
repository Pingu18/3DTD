using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject colliderSpawnPoint;
    [SerializeField] private GameObject colliderObj;
    [SerializeField] private Animator playerAnim;
    private PlayerObject playerObj;
    private BuildController buildCon;
    private Teleport teleport;

    private float fireTime;
    private float fireCD = 0.1f;

    private bool canUseFlamethrower;

    private void Start()
    {
        buildCon = FindObjectOfType<BuildController>();
        playerObj = FindObjectOfType<PlayerObject>();
        teleport = FindObjectOfType<Teleport>();
        canUseFlamethrower = true;
    }

    private void Update()
    {
        if (!buildCon.getInBuild())
        {
            secondaryAttack();
        }
    }

    private void secondaryAttack()
    {
        if (Input.GetMouseButtonDown(1) && playerObj.getMana() >= 1 && canUseFlamethrower)
        {
            playerAnim.ResetTrigger("StopRChannel");
            playerAnim.SetTrigger("StartRChannel");
            playerAnim.SetBool("ChannelingR", true);
            transform.GetComponent<VisualEffect>().Play();
        }

        if (Input.GetMouseButtonUp(1))
        {
            playerAnim.SetTrigger("StopRChannel");
            playerAnim.SetBool("ChannelingR", false);
            transform.GetComponent<VisualEffect>().Stop();
        }

        if (Input.GetMouseButton(1) && Time.time > fireTime)
        {
            if (playerObj.getMana() >= 1.0f && canUseFlamethrower)
            {
                if (!playerAnim.GetBool("ChannelingR"))
                {
                    playerAnim.ResetTrigger("StopRChannel");
                    playerAnim.SetTrigger("StartRChannel");
                    playerAnim.SetBool("ChannelingR", true);
                }
                transform.GetComponent<VisualEffect>().Play();
                playerObj.useMana(1.0f);
                Instantiate(colliderObj, colliderSpawnPoint.transform.position, colliderSpawnPoint.transform.rotation);
                fireTime = Time.time + fireCD;
            }
            else
            {
                playerAnim.SetTrigger("StopRChannel");
                playerAnim.SetBool("ChannelingR", false);
                transform.GetComponent<VisualEffect>().Stop();
            }

        }
    }

    public void setCanUseFlamethrower(bool canUse)
    {
        canUseFlamethrower = canUse;
    }
}
