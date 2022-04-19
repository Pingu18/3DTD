using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera buildCamera;
    [SerializeField] private GameObject mouseIndicatorHighlight;

    public float camSpeed = 20f;
    public float panBorder = 10f;
    public Vector2 panLimit;

    private bool inBuild = false;
    
    private void Update()
    {
        ToggleBuild();

        if (inBuild)
        {
            MoveCam();
            MouseIndicator();
        } else
        {
            mouseIndicatorHighlight.SetActive(false);
        }
    }

    private void MoveCam()
    {
        Vector3 pos = buildCamera.transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorder)
        {
            pos.z += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorder)
        {
            pos.z -= camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorder)
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorder)
        {
            pos.x -= camSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        buildCamera.transform.position = pos;
    }

    private void ToggleBuild()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuild = !inBuild;
        }
    }

    private void MouseIndicator()
    {
        mouseIndicatorHighlight.SetActive(true);
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            transform.position = raycastHit.point;
        }
        else
        {
            mouseIndicatorHighlight.SetActive(false);
        }
    }
}
