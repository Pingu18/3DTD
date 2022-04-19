using UnityEngine;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    [SerializeField] private Camera buildCamera;
    [SerializeField] private GameObject mouseIndicatorHighlight;
    public GameObject towerBase;
    public GameObject tower2;
    private GameObject activeStructure;

    public Canvas buildCanvas;
    private int activeSlot;
    public Image Slot1;
    public Image Slot2;

    private Color32 defaultColor = new Color32(255, 255, 255, 255);
    private Color32 activeColor = new Color32(115, 255, 128, 255);

    public float camSpeed = 20f;
    public float panBorder = 10f;
    public Vector2 panLimit;

    private bool inBuild = false;

    private void Start()
    {
        activeStructure = towerBase;
        activeSlot = 1;
    }

    private void Update()
    {
        ToggleBuild();

        if (inBuild)
        {
            MoveCam();
            MouseIndicator();
            StructurePlacement();
            BuildUIControl();
            buildCanvas.gameObject.SetActive(true);
        } else
        {
            mouseIndicatorHighlight.SetActive(false);
            buildCanvas.gameObject.SetActive(false);
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

    private void StructurePlacement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeStructure = towerBase;
            activeSlot = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeStructure = tower2;
            activeSlot = 2;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(activeStructure, transform.position, Quaternion.identity);
        }
    }

    private void BuildUIControl()
    {
        switch (activeSlot) 
        {
            case 1:
                Slot1.color = activeColor;
                Slot2.color = defaultColor;
                break;
            case 2:
                Slot1.color = defaultColor;
                Slot2.color = activeColor;
                break;
            default:
                break;
        }
    }
}
