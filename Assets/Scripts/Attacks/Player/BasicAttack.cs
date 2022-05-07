using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;

    /*
    private PlayerController playerController;

    [SerializeField] private GameObject currentAttackContainer;
    [SerializeField] private Rigidbody currentRigidbody;

    [SerializeField] private GameObject fireAttackContainer;

    [SerializeField] private ParticleSystem collisionParticleSystem;

    private List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();

    [Header("Player Orientation")]
    [SerializeField] private Transform orientation;     // reference to player orientation
    */

    void Start()
    {
        //playerController = GetComponent<PlayerController>();

        effectToSpawn = vfx[0];
    }

    public void spawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
        else
            Debug.Log("No fire point...");
    }

    /*
    public void fireBasicAttack()
    {
        for (int i = 0; i < fireAttackContainer.transform.childCount; i++)
        {
            ParticleSystem currentParticleSystem = fireAttackContainer.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();

            currentParticleSystem.Play();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        for (int i = 0; i < currentAttackContainer.transform.childCount; i++)
        {
            ParticleSystem currentParticleSystem = currentAttackContainer.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();

            currentParticleSystem.Stop();
        }
    }
    */
}
