using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerCollider : MonoBehaviour
{
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private float speed;
    [SerializeField] private float duration;

    private void Update()
    {
        if (duration > 0)
            duration -= Time.deltaTime;

        if (duration <= 0)
            Destroy(this.gameObject);

        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageable>().queueDamage(1f, null, true);
        }
    }
}
