using UnityEngine;

public interface IDamageable
{
    void queueDamage(float dmgTaken, GameObject tower, bool playerDamage);
}