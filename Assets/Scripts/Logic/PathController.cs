using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    [SerializeField] private GameObject activePath;
    [SerializeField] private GameObject inactivePath;

    private List<Transform> currentPath = new List<Transform>();

    private void Start()
    {
        Debug.Log("Building paths... (PathController)");

        navMeshSurface = activePath.GetComponent<NavMeshSurface>();
    }

    public bool checkPathAdded(int pathNum)
    {
        for (int i = 0; i < currentPath.Count; i++)
        {
            if (currentPath[i].name.Equals(("Path" + pathNum).ToString()))
                return true;
        }

        return false;
    }

    public void addPath(int pathNum)
    {
        for (int i = 0; i < inactivePath.transform.childCount; i++)
        {
            Transform childTransform = inactivePath.transform.GetChild(i);

            if (childTransform.name.Equals(("Path" + pathNum).ToString()))
            {
                childTransform.parent = activePath.transform;
                currentPath.Add(childTransform);
                break;
            }
        }

        navMeshSurface.BuildNavMesh();
    }

    public void clearPaths()
    {
        for (int i = 0; i < currentPath.Count; i++)
        {
            currentPath[i].parent = inactivePath.transform;
        }

        currentPath.Clear();
    }
}
