using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRamp : MonoBehaviour
{
    private GameObject target;

    [SerializeField] private float timer;
    private float damageMultiplier;

    void Start()
    {
        damageMultiplier = 1f;
    }

    void Update()
    {
        if (timer < 10 && target != null)
        {
            timer += Time.deltaTime;
            damageMultiplier = 1 + ((timer / 10) * GetComponent<TowerObject>().getSpecial());
        } else if (timer > 10 && target != null)
        {
            timer = 10f;
            damageMultiplier = 1 + ((timer / 10) * GetComponent<TowerObject>().getSpecial());
        }
    }

    public float getDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void setTarget(GameObject newTarget)
    {
        if (target != newTarget)
        {
            target = newTarget;
            resetDamage();
        }
    }

    private void resetDamage()
    {
        damageMultiplier = 1f;
        timer = 0;
    }
}
