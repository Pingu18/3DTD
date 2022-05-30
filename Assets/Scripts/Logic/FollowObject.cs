using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject toFollow;

    private void Update()
    {
        if (toFollow != null)
        {
            this.transform.position = toFollow.transform.position;
        }
    }
}
