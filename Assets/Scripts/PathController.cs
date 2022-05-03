using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private int differentPaths;
    private int currentPath = 1;

    [SerializeField] private NavMeshSurface[] paths;
    [SerializeField] private NavMeshSurface[] surfaces;

    private void Start()
    {
        Debug.Log("Building paths... (PathController)");

        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }

        buildPath();
    }

    private void buildPath()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            if (paths[i].name == ("LeftPath" + currentPath).ToString() || paths[i].name == ("RightPath" + currentPath).ToString())
            {
                paths[i].defaultArea = 0;
            } else
            {
                paths[i].defaultArea = 1;
            }

            Debug.Log("Building NavMesh...");
            paths[i].BuildNavMesh();
        }
    }

    private void togglePaths()
    {
        Debug.Log("Toggling paths...");

        if (currentPath == 1)
        {
            currentPath = 2;
            buildPath();
        }
        else
        {
            currentPath = 1;
            buildPath();
        }
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
