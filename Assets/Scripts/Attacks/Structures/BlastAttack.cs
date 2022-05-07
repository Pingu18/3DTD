using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastAttack : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        if (target != null)
        {
            this.gameObject.transform.position = target.transform.position;
        }
    }
}
