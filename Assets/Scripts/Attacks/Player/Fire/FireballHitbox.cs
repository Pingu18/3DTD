using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHitbox : MonoBehaviour
{
    [SerializeField] private GameObject fireballRadius;

    private GameObject fireball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            fireball = Instantiate(fireballRadius, other.gameObject.transform.position, Quaternion.identity);

            fireball.GetComponent<FireballCollider>().setInitialTarget(other.gameObject);
            other.gameObject.GetComponent<IDamageable>().queueDamage(15f, null, true);

            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}