using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollider : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private GameObject initialTarget;

    private float newRadius;
    [SerializeField] private float radius;

    private void Start()
    {
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
    }
}
