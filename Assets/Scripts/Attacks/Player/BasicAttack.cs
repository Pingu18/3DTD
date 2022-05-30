using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private GameObject firePoint;
    [SerializeField] private List<GameObject> vfx = new List<GameObject>();

    [SerializeField] private GameObject fireballCollider;

    private GameObject effectToSpawn;
    private AudioController audioCon;

    void Start()
    {
        effectToSpawn = vfx[0];
        audioCon = FindObjectOfType<AudioController>();
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
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
            audioCon.PlaySound("FireBasicAtk", vfx);

        } else
        {
            Debug.Log("No fire point...");
        }
    }

    public GameObject getFireballCollider()
    {
        return fireballCollider;
    }
}
