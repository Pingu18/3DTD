using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerUpgrades towerUpgrades;
    [SerializeField] private GameObject currencyContainer;

    private CurrencyController currencyController;

    [SerializeField] private GameObject selectedTower;
    private TowerObject towerObj;
    private Slow slow;

    private TMP_Text text;
    private Image bar;
    private Button button;

    private Color upgradedColor;

    private RectTransform statTransform;
    private RectTransform upgradeTransform;

    private float bookmarkMaxHeight;
    private float bookmarkMinHeight;

    [Header("Screens")]
    [SerializeField] private GameObject statScreen;
    [SerializeField] private GameObject upgradeScreen;

    [Header("Screen Buttons")]
    [SerializeField] private Button statButton;
    [SerializeField] private Button upgradeButton;

    [Header("Menu Stats")]
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerElementText;

    [Header("Row Containers")]
    [SerializeField] private Transform rowTexts;
    [SerializeField] private Transform rowDescs;

    [Header("Bar Containers")]
    [SerializeField] private Transform upgradeBars1;
    [SerializeField] private Transform upgradeBars2;
    [SerializeField] private Transform upgradeBars3;
    [SerializeField] private Transform upgradeBars4;

    [Header("Button Containers")]
    [SerializeField] private Transform buttonContainer;

    [Header("Animations")]
    [SerializeField] private Animator upgradeMenuAnimation;

    private void Start()
    {
        towerUpgrades = GetComponent<TowerUpgrades>();
        currencyController = currencyContainer.GetComponent<CurrencyController>();

        statTransform = statButton.GetComponent<RectTransform>();
        upgradeTransform = upgradeButton.GetComponent<RectTransform>();

        bookmarkMaxHeight = statTransform.transform.localPosition.y + 5;
        bookmarkMinHeight = statTransform.transform.localPosition.y;

        upgradedColor = new Color(  0f / 255f, 255f / 255f,  15f / 255f);

        toggleStatsScreen();
    }

    public void toggleStatsScreen()
    {
        statScreen.SetActive(true);
        upgradeScreen.SetActive(false);

        statTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
        statTransform.transform.localPosition = new Vector3(statTransform.transform.localPosition.x, bookmarkMaxHeight, statTransform.transform.localPosition.z);

        upgradeTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        upgradeTransform.transform.localPosition = new Vector3(upgradeTransform.transform.localPosition.x, bookmarkMinHeight, upgradeTransform.transform.localPosition.z);
    }

    public void toggleUpgradesScreen()
    {
        statScreen.SetActive(false);
        upgradeScreen.SetActive(true);

        statTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        statTransform.transform.localPosition = new Vector3(statTransform.transform.localPosition.x, bookmarkMinHeight, statTransform.transform.localPosition.z);

        upgradeTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
        upgradeTransform.transform.localPosition = new Vector3(upgradeTransform.transform.localPosition.x, bookmarkMaxHeight, upgradeTransform.transform.localPosition.z);
    }

    public void upgradeDamage()
    {
        int cost = towerUpgrades.getCost("level_" + (towerObj.getDMGLevel() + 1) + "_dmgCost");
        Debug.Log(cost);
    }

    public void setIsSelected(bool c)
    {
        upgradeMenuAnimation.SetBool("isSelected", c);
    }

    public void startTrigger(string triggerName)
    {
        upgradeMenuAnimation.SetTrigger(triggerName);
    }

    public void setUpgradeMenu()
    {
        Debug.Log("Setting upgrade menu...");

        towerNameText.text      = towerObj != null ? towerObj.getName() : "";
        towerElementText.text   = towerObj != null ? "Element: " + towerObj.getElement() : "";

        string[] upgrades = towerObj.getUpgrades();
        int numUpgrades = upgrades.Length;
        string costName;

        for (int i = 0; i < numUpgrades; i++)
        {
            TMP_Text rowText = rowTexts.GetChild(i).GetComponent<TMP_Text>();
            TMP_Text rowDesc = rowDescs.GetChild(i).GetComponent<TMP_Text>();

            switch (upgrades[i])
            {
                case "damage":
                    costName = "level_" + (towerObj.getDMGLevel() + 1).ToString() + "_dmgCost";
                    rowText.text = "Damage: " + towerObj.getDamage().ToString();
                    rowDesc.text = "How much damage the tower deals";
                    setBars((i + 1), towerObj.getDMGLevel());
                    setButtons(i, towerUpgrades.getCost(costName));
                    break;
                case "firerate":
                    costName = "level_" + (towerObj.getFireRateLevel() + 1).ToString() + "_frCost";
                    rowText.text = "Fire Rate: " + towerObj.getFireRate().ToString();
                    rowDesc.text = "How fast the tower attacks";
                    setBars((i + 1), towerObj.getFireRateLevel());
                    setButtons(i, towerUpgrades.getCost(costName));
                    break;
                case "range":
                    costName = "level_" + (towerObj.getRangeLevel() + 1).ToString() + "_rangeCost";
                    rowText.text = "Range: " + towerObj.getRange().ToString();
                    rowDesc.text = "How far the tower can attack";
                    setBars((i + 1), towerObj.getRangeLevel());
                    setButtons(i, towerUpgrades.getCost(costName));
                    break;
                case "slowPercent":
                    slow = selectedTower.GetComponent<Slow>();
                    costName = "level_" + (slow.getSlowPercentLevel() + 1).ToString() + "_slowPercentCost";
                    rowText.text = "Slow Percent: " + slow.getSlowPercent().ToString();
                    rowDesc.text = "How much the tower slows enemies on attack";
                    setBars((i + 1), slow.getSlowPercentLevel());
                    setButtons(i, towerUpgrades.getCost(costName));
                    break;
                case "slowDuration":
                    slow = selectedTower.GetComponent<Slow>();
                    costName = "level_" + (slow.getSlowDurationLevel() + 1).ToString() + "_slowDurCost";
                    rowText.text = "Slow Duration: " + slow.getSlowDuration().ToString();
                    rowDesc.text = "How long the enemies get slowed for";
                    setBars((i + 1), slow.getSlowDurationLevel());
                    setButtons(i, towerUpgrades.getCost(costName));
                    break;
                case "special":
                    if (towerObj.getSpecialLevel() == 0)
                    {
                        rowText.text = "Unlock Special";
                        rowDesc.text = towerObj.getSpecialDesc();
                    } else
                    {
                        rowText.text = "Special: " + towerObj.getSpecial().ToString();
                        rowDesc.text = towerObj.getSpecialUpgradeDesc();
                        setBars((i + 1), towerObj.getSpecialLevel());
                    }
                    break;
                default:
                    rowText.text = "";
                    rowDesc.text = "";
                    break;
            }
        }
    }

    public void setBars(int row, int currentLevel)
    {
        switch (row)
        {
            case 1:
                for (int i = 0; i < currentLevel; i++)
                {
                    bar = upgradeBars1.GetChild(i).GetComponent<Image>();
                    bar.color = upgradedColor;
                }
                break;
            case 2:
                for (int i = 0; i < currentLevel; i++)
                {
                    bar = upgradeBars2.GetChild(i).GetComponent<Image>();
                    bar.color = upgradedColor;
                }
                break;
            case 3:
                for (int i = 0; i < currentLevel; i++)
                {
                    bar = upgradeBars3.GetChild(i).GetComponent<Image>();
                    bar.color = upgradedColor;
                }
                break;
            case 4:
                for (int i = 0; i < currentLevel; i++)
                {
                    bar = upgradeBars4.GetChild(i).GetComponent<Image>();
                    bar.color = upgradedColor;
                }
                break;
            default:
                break;
        }
    }

    public void setButtons(int row, int cost)
    {
        TMP_Text text = buttonContainer.GetChild(row).GetComponentInChildren<TMP_Text>();
        text.text = cost.ToString();
    }

    public void hideBars()
    {

    }

    public void emptySelectedTower()
    {
        selectedTower = null;
    }

    public void setSelectedTower(GameObject obj)
    {
        selectedTower = obj;
        towerObj = selectedTower.GetComponent<TowerObject>();

        hideBars();
        setUpgradeMenu();
    }
}
