using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] private GameObject colliderSpawnPoint;
    [SerializeField] private GameObject colliderObj;
    private BuildController buildCon;

    private float fireTime;
    private float fireCD = 0.1f;

    private void Start()
    {
        buildCon = FindObjectOfType<BuildController>();
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
        if (Input.GetMouseButtonDown(1))
        {
            transform.GetComponent<VisualEffect>().Play();
        }

        if (Input.GetMouseButtonUp(1))
        {
            transform.GetComponent<VisualEffect>().Stop();
        }

        if (Input.GetMouseButton(1) && Time.time > fireTime)
        {
            Instantiate(colliderObj, colliderSpawnPoint.transform.position, colliderSpawnPoint.transform.rotation);
            fireTime = Time.time + fireCD;
        }
    }
}
