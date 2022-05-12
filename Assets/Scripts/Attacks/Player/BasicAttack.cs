using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;

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
    private void OnParticleCollision(GameObject other)
    {
        
    }
    */
}
