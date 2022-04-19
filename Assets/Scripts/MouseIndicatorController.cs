using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIndicatorController : MonoBehaviour
{
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Material cannotPlaceMat;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    public bool canPlace;

    private void Start()
    {
        Physics.IgnoreLayerCollision(0, 6);
    }

    private void Update()
    {
        if (collisions.Count > 0)
        {
            canPlace = false;
            this.gameObject.GetComponent<MeshRenderer>().material = cannotPlaceMat;
        }
        else
        {
            canPlace = true;
            this.gameObject.GetComponent<MeshRenderer>().material = canPlaceMat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Started colliding with: " + other.gameObject.name);
        collisions.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        print("Stopped colliding with: " + other.gameObject.name);
        collisions.Remove(other.gameObject);
    }
}
