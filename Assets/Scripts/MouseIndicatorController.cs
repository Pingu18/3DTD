using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIndicatorController : MonoBehaviour
{
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Material cannotPlaceMat;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    public bool canPlace = true;
    MeshRenderer meshRenderer;
    public Color32 canPlaceColor = new Color32(0, 255, 255, 100);
    public Color32 cannotPlaceColor = new Color32(255, 5, 0, 100);

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 6);
        Physics.IgnoreLayerCollision(9, 0);
        Physics.IgnoreLayerCollision(9, 9);
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        collisions.Add(other.gameObject);
        canPlace = false;
        meshRenderer.material.color = cannotPlaceColor;
    }

    private void OnTriggerExit(Collider other)
    {
        collisions.Remove(other.gameObject);
        if (collisions.Count > 0)
        {
            canPlace = false;
            meshRenderer.material.color = cannotPlaceColor;
        }
        else
        {
            canPlace = true;
            meshRenderer.material.color = canPlaceColor;
        }
    }

    public void RemoveStructureFromCollisions(GameObject structure)
    {
        collisions.Remove(structure);
    }

    public void ClearCollisions()
    {
        collisions.Clear();
    }
}
