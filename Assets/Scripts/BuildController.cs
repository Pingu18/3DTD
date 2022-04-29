using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BuildController : MonoBehaviour
{
    [SerializeField] private Camera buildCamera;
    [SerializeField] private GameObject activeIndicator;
    [SerializeField] private GameObject mouseIndicatorHighlight;
    [SerializeField] private GameObject iceTowerIndicator;
    [SerializeField] private GameObject fireTowerIndicator;
    [SerializeField] private GameObject grassTowerIndicator;
    [SerializeField] private GameObject lightningTowerIndicator;
    [SerializeField] private GameObject lightTowerIndicator;
    [SerializeField] private GameObject darkTowerIndicator;

    [SerializeField] private LayerMask placeableLayerMask; // set to Ground layer so can only place structures on ground
    [SerializeField] private LayerMask structureLayerMask;

    [SerializeField] private GameObject towerContainer;

    [SerializeField] private GameObject cameraControllerObj;    // reference to GameObject that holds CameraController script
    private CameraController cameraController;  // reference to CameraController script
    private MouseIndicatorController mouseCon;

    [SerializeField] private GameObject currencyContainer;
    private CurrencyController currencyController;

    [SerializeField] private MeshRenderer meshRenderer;

    private Color32 defaultColor = new Color32(255, 255, 255, 255);
    private Color32 activeColor = new Color32(115, 255, 128, 255);

    [Header("Debugging")]
    [SerializeField] private TowerController towerController;
    [SerializeField] private GameObject towerObj;
    [SerializeField] private TowerObject selectedTower;

    [Header("Structures")]
    public GameObject iceTower;
    public GameObject fireTower;
    public GameObject grassTower;
    public GameObject lightningTower;
    public GameObject lightTower;
    public GameObject darkTower;
    private GameObject activeStructure;

    [Header("UI")]
    public Canvas buildCanvas;
    private int activeSlot;
    public Image Slot1;
    public Image Slot2;
    public Image Slot3;
    public Image Slot4;
    public Image Slot5;
    public Image Slot6;
    public TMP_Text modeText;
    public TMP_Text poorText;

    [Header("Camera Controls")]
    public float camSpeed = 20f;
    public float scrollSpeed = 25f;
    public float panBorder = 10f;
    public Vector2 panLimit;
    private float scroll;
    public float minY = 20f;
    public float maxY = 80f;

    [Header("Animations")]
    public Animator buildModeAnim;

    enum BuildMode
    {
        PLACE,
        DELETE,
        UPGRADE
    }

    private bool inBuild = false;
    private BuildMode buildMode = BuildMode.PLACE;

    private void Start()
    {
        activeStructure = iceTower; // default tower selected
        activeSlot = 1; // default tower highlighted on HUD

        iceTowerIndicator.SetActive(true);
        mouseIndicatorHighlight.SetActive(false);

        //buildCanvas.gameObject.SetActive(false);
        modeText.gameObject.SetActive(false);

        UpdateSlotsUI();
        buildCanvas.gameObject.SetActive(true);
        initializeIndicator();

        cameraController = cameraControllerObj.GetComponent<CameraController>();
        currencyController = currencyContainer.GetComponent<CurrencyController>();
        towerController = towerContainer.GetComponent<TowerController>();
        meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        ToggleBuild(); // press tab to toggle build mode

        if (inBuild)
        {
            ToggleMode(); // toggle between placing and deleting structures
            MoveCam(); // pan camera with WASD or mouse + zooming
            BuildUIControl(); // selecting towers + UI elements

            if (buildMode == BuildMode.PLACE)
            {
                MouseIndicator(); // handle structure placement previews
                StructurePlacement(); // handle placement of structures
            }
            if (buildMode == BuildMode.DELETE)
            {
                StructureDeletion(); // handle deletion of structures
            }
        }
    }

    private void MoveCam()
    {
        Vector3 pos = buildCamera.transform.position; // current camera position
                                                      // Quaternion rot = buildCamera.transform.rotation;

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

        scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x); // limit area you can pan to (can't move camera forever)
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        buildCamera.transform.position = pos; // update with new camera position
    }

    private void ToggleBuild()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuild = !inBuild;

            if (inBuild)
            {
                buildModeAnim.SetBool("inBuild", true);
            }
            else
            {
                buildModeAnim.SetBool("inBuild", false);
                unselectTower();
            }

            activeIndicator.SetActive(!activeIndicator.activeSelf);
            mouseCon.ClearCollisions();
            modeText.gameObject.SetActive(!modeText.gameObject.activeSelf);

            cameraController.toggleCamera();
            cameraController.toggleMouseLock();
            cameraController.toggleCanvas();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                meshRenderer.enabled = false;
            } else if (buildMode == BuildMode.DELETE)
            {
                buildMode = BuildMode.PLACE;
                mouseCon.UpdateCollisions();
                modeText.text = "PLACING";
            }
        }
    }

    private void MouseIndicator()
    {
        meshRenderer.enabled = true;
        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, placeableLayerMask))
        {
            transform.position = raycastHit.point;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void StructurePlacement()
    {
        // use numbers to swap between towers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeStructure = iceTower;
            activeSlot = 1;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeStructure = fireTower;
            activeSlot = 2;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeStructure = grassTower;
            activeSlot = 3;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeStructure = lightningTower;
            activeSlot = 4;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activeStructure = lightTower;
            activeSlot = 5;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            activeStructure = darkTower;
            activeSlot = 6;
            UpdateSlotsUI();
            meshRenderer = activeIndicator.GetComponent<MeshRenderer>();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseCon.canPlace)
        {
            int cost = activeStructure.GetComponentInChildren<TowerObject>().getCost();

            if (currencyController.checkSufficientMoney(cost))
            {
                Instantiate(activeStructure, transform.position, Quaternion.identity).transform.parent = towerContainer.transform; // place tower at mouse location
                disablePoorText();
            } else {
                showPoorText();
            }
        }
    }

    private void StructureDeletion()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition); // shoot ray from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, structureLayerMask))
            {
                GameObject obj = raycastHit.collider.gameObject;
                mouseCon.RemoveStructureFromCollisions(obj);
                if (raycastHit.collider.gameObject == towerObj)
                {
                    towerObj = null;
                    selectedTower = null;
                    towerController.setIsSelected(false);
                    towerController.startTrigger("deselect");
                }
                currencyController.addMoney(obj.GetComponent<TowerObject>().getResaleValue());
                Destroy(obj);   
            }
        }
    }

    private void BuildUIControl()
    {
        // select and unselected towers
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, structureLayerMask))
            {
                if (raycastHit.transform.gameObject.GetComponent<TowerObject>())
                {
                    towerObj = raycastHit.transform.gameObject;

                    if (selectedTower != towerObj.GetComponent<TowerObject>() && selectedTower != null)
                        selectedTower.setOutline(false);

                    selectedTower = towerObj.GetComponent<TowerObject>();
                    selectedTower.setOutline(true);

                    towerController.setSelectedTower(towerObj);
                    towerController.setIsSelected(true);

                    buildMode = BuildMode.UPGRADE;
                    modeText.text = "UPGRADING";
                    activeIndicator.SetActive(false);
                }
            } else
            {
                if (selectedTower != null)
                    unselectTower();
            }
        }
    }

    private void unselectTower()
    {
        if (selectedTower != null)
            selectedTower.setOutline(false);

        towerObj = null;

        towerController.emptySelectedTower();
        towerController.setIsSelected(false);
        towerController.startTrigger("deselect");

        buildMode = BuildMode.PLACE;
        modeText.text = "PLACING";
        activeIndicator.SetActive(true);

        mouseCon.ClearCollisions();
        mouseCon.UpdateCollisions();
    }

    private void initializeIndicator()
    {
        activeIndicator.SetActive(false);
    }

    private void UpdateSlotsUI()
    {
        switch (activeSlot)
        {
            case 1:
                activeIndicator = iceTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(true);
                fireTowerIndicator.SetActive(false);
                grassTowerIndicator.SetActive(false);
                lightTowerIndicator.SetActive(false);
                mouseIndicatorHighlight.SetActive(false);
                Slot1.color = activeColor;
                Slot2.color = defaultColor;
                Slot3.color = defaultColor;
                Slot4.color = defaultColor;
                Slot5.color = defaultColor;
                Slot6.color = defaultColor;
                break;
            case 2:
                activeIndicator = fireTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(false);
                fireTowerIndicator.SetActive(true);
                grassTowerIndicator.SetActive(false);
                lightTowerIndicator.SetActive(false);
                mouseIndicatorHighlight.SetActive(false);
                Slot1.color = defaultColor;
                Slot2.color = activeColor;
                Slot3.color = defaultColor;
                Slot4.color = defaultColor;
                Slot5.color = defaultColor;
                Slot6.color = defaultColor;
                break;
            case 3:
                activeIndicator = grassTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(false);
                mouseIndicatorHighlight.SetActive(false);
                fireTowerIndicator.SetActive(false);
                grassTowerIndicator.SetActive(true);
                lightTowerIndicator.SetActive(false);
                Slot1.color = defaultColor;
                Slot2.color = defaultColor;
                Slot3.color = activeColor;
                Slot4.color = defaultColor;
                Slot5.color = defaultColor;
                Slot6.color = defaultColor;
                break;
            case 4:
                activeIndicator = mouseIndicatorHighlight;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(false);
                fireTowerIndicator.SetActive(false);
                grassTowerIndicator.SetActive(false);
                lightTowerIndicator.SetActive(false);
                mouseIndicatorHighlight.SetActive(true);
                Slot1.color = defaultColor;
                Slot2.color = defaultColor;
                Slot3.color = defaultColor;
                Slot4.color = activeColor;
                Slot5.color = defaultColor;
                Slot6.color = defaultColor;
                break;
            case 5:
                activeIndicator = lightTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(false);
                fireTowerIndicator.SetActive(false);
                grassTowerIndicator.SetActive(false);
                lightTowerIndicator.SetActive(true);
                mouseIndicatorHighlight.SetActive(false);
                Slot1.color = defaultColor;
                Slot2.color = defaultColor;
                Slot3.color = defaultColor;
                Slot4.color = defaultColor;
                Slot5.color = activeColor;
                Slot6.color = defaultColor;
                break;
            case 6:
                activeIndicator = lightTowerIndicator;
                mouseCon = activeIndicator.GetComponent<MouseIndicatorController>();
                iceTowerIndicator.SetActive(false);
                fireTowerIndicator.SetActive(false);
                grassTowerIndicator.SetActive(false);
                lightTowerIndicator.SetActive(true);
                mouseIndicatorHighlight.SetActive(false);
                Slot1.color = defaultColor;
                Slot2.color = defaultColor;
                Slot3.color = defaultColor;
                Slot4.color = defaultColor;
                Slot5.color = defaultColor;
                Slot6.color = activeColor;
                break;
            default:
                break;
        }

        mouseCon.ClearCollisions();
    }

    public void disablePoorText()
    {
        poorText.enabled = false;
    }

    public void showPoorText()
    {
        poorText.enabled = true;
        StartCoroutine(wait(3));
        poorText.enabled = false;
    }

    private IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        poorText.enabled = false;
    }
    public bool getInBuild()
    {
        return inBuild;
    }
}
