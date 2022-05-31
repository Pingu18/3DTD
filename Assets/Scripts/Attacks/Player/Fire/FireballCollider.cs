using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollider : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private GameObject initialTarget;
    private PlayerObject playerObj;

    private float newRadius;
    [SerializeField] private float radius;

    private void Start()
    {
        playerObj = FindObjectOfType<PlayerObject>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    public void setInitialTarget(GameObject target)
    {
        initialTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && other.gameObject != initialTarget)
        {
            other.gameObject.GetComponent<IDamageable>().queueDamage(10f, null, true);
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Enemy") && other.gameObject == initialTarget)
        {
            playerObj.addMana(3f);
        }
    }
}
