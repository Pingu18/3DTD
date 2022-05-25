using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    //[SerializeField] private new ParticleSystem particleSystem;

    [SerializeField] private float speed;
    [SerializeField] private float fireRate;
    [SerializeField] private float duration;

    void Start()
    {
        //ParticleSystem.MainModule main = particleSystem.main;

        //main.startSpeed = speed;
    }

    void Update()
    {
        if (duration > 0)
            duration -= Time.deltaTime;

        if (duration <= 0)
            Destroy(this.gameObject);

        transform.position += transform.forward * (speed * Time.deltaTime);
    }
}
