using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildController : MonoBehaviour
{
    [SerializeField] private Camera buildCamera;
    [SerializeField] private GameObject mouseIndicatorHighlight;
    [SerializeField] private LayerMask groundLayerMask; // set to Ground layer so can only place structures on ground
    [SerializeField] private LayerMask structureLayerMask;

    MouseIndicatorController mouseCon;
    public GameObject towerBase;
    public GameObject tower2;
    private GameObject activeStructure;

    public Canvas buildCanvas;
    private int activeSlot;
    public Image Slot1;
    public Image Slot2;
    public TMP_Text modeText;

    private Color32 defaultColor = new Color32(255, 255, 255, 255);
    private Color32 activeColor = new Color32(115, 255, 128, 255);

    public float camSpeed = 20f;
    public float panBorder = 10f;
    public Vector2 panLimit;

    enum BuildMode
    {
        PLACE,
        DELETE
    }

    private bool inBuild = false;
    private BuildMode buildMode = BuildMode.PLACE;

    private void Start()
    {
        activeStructure = towerBase; // default tower selected
        activeSlot = 1; // default tower highlighted on HUD
        mouseCon = FindObjectOfType<MouseIndicatorController>();
    }

    private void Update()
    {
        ToggleBuild(); // press tab to toggle build mode

        if (inBuild)
        {
            ToggleMode();
            MoveCam();
            BuildUIControl();
            buildCanvas.gameObject.SetActive(true);
            modeText.gameObject.SetActive(true);

            if (buildMode == BuildMode.PLACE)
            {
                MouseIndicator();
                StructurePlacement();
            }
            if (buildMode == BuildMode.DELETE)
            {
                StructureDeletion();
                mouseIndicatorHighlight.SetActive(false);
                ;
            }
        } else
        {
            mouseIndicatorHighlight.SetActive(false);
            buildCanvas.gameObject.SetActive(false);
            modeText.gameObject.SetActive(false);
        }
    }

    private void MoveCam()
    {
        Vector3 pos = buildCamera.transform.position; // current camera position

        // use WASD or mouse to pan camera
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

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x); // limit area you can pan to (can't move camera forever)
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        buildCamera.transform.position = pos; // update with new camera position
    }

    private void ToggleBuild()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuild = !inBuild;
        }
    }

    private void ToggleMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (buildMode == BuildMode.PLACE)
            {
                buildMode = BuildMode.DELETE;
                modeText.text = "REMOVING";
            } else if (buildMode == BuildMode.DELETE)
            {
                buildMode = BuildMode.PLACE;
                modeText.text = "PLACING";
            }
        }
    }

    private void MouseIndicator()
    {
        mouseIndicatorHighlight.SetActive(true);
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayerMask))
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
        // use numbers to swap between towers
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseCon.canPlace)
        {
            Instantiate(activeStructure, transform.position, Quaternion.identity); // place tower at mouse location
        }
    }

    private void StructureDeletion()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, structureLayerMask))
            {
                Destroy(raycastHit.collider.gameObject);
            }
        }
    }

    private void BuildUIControl()
    {
        // handle UI for build mode
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
