using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    [SerializeField] private GameObject activePath;
    [SerializeField] private GameObject inactivePath;

    private Transform currentPath;
    private int currentPathNum;

    private void Start()
    {
        Debug.Log("Building paths... (PathController)");

        navMeshSurface = activePath.GetComponent<NavMeshSurface>();
        currentPathNum = 1;

        buildPath();
    }

    private void buildPath()
    {
        for (int i = 0; i < inactivePath.transform.childCount; i++)
        {
            Transform childTransform = inactivePath.transform.GetChild(i);

            if (childTransform.name.Equals(("Path" + currentPathNum).ToString()))
            {
                childTransform.parent = activePath.transform;
                currentPath = childTransform;
                break;
            }
        }

        navMeshSurface.BuildNavMesh();
    }

    private void togglePaths()
    {
        Debug.Log("Toggling paths...");

        if (currentPathNum == 1)
            currentPathNum = 2;
        else if (currentPathNum == 2)
            currentPathNum = 1;

        currentPath.parent = inactivePath.transform;
        buildPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            togglePaths();
        }
    }
}
