using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private List<GameObject> vfx = new List<GameObject>();

    [SerializeField] private GameObject fireSlashCollider;

    private GameObject effectToSpawn;

    void Start()
    {
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

    public IEnumerator spawnVFX(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject vfx;

        if (firePoint != null)
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
        else
            Debug.Log("No fire point...");
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Test");
    }

    public GameObject getFireSlashCollider()
    {
        return fireSlashCollider;
    }
}
