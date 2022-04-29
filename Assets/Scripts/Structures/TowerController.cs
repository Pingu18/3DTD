using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerController : MonoBehaviour
{
    // Container references
    [SerializeField] private GameObject currencyContainer;
    [SerializeField] private GameObject buildContainer;

    // Script references
    private CurrencyController currencyController;
    private BuildController buildController;

    // Automatic script references
    private TowerUpgrades towerUpgrades;
    private TowerObject towerObj;

    // Automatic references
    private GameObject selectedTower;

    // Tower Abilites
    private Heal heal;
    private Slow slow;

    // Variables
    private TMP_Text text;
    private Image bar;
    private Button button;
    private RectTransform statTransform;
    private RectTransform upgradeTransform;
    private Color upgradedColor;
    private Color notUpgradedColor;
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
        buildController = buildContainer.GetComponent<BuildController>();

        statTransform = statButton.GetComponent<RectTransform>();
        upgradeTransform = upgradeButton.GetComponent<RectTransform>();

        bookmarkMaxHeight = statTransform.transform.localPosition.y + 5;
        bookmarkMinHeight = statTransform.transform.localPosition.y;

        upgradedColor    = new Color(  0f / 255f, 255f / 255f,  15f / 255f);
        notUpgradedColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        toggleStatsScreen();
    }

    // Toggle screen functions
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

    // Upgrade functions
    public void upgradeDamage(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (towerObj.getDMGLevel() + 1).ToString() + "_newDmg_" + towerObj.getElement();
        float newDamage = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower stats
            towerObj.setBaseDamage((int)newDamage);
            towerObj.setDMGLevel(towerObj.getDMGLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (towerObj.getDMGLevel() + 1).ToString() + "_dmgCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Damage: " + towerObj.getDamage().ToString();
            highlightBars(row, towerObj.getDMGLevel(), 5);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        } else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeFireRate(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (towerObj.getFireRateLevel() + 1).ToString() + "_newFr_" + towerObj.getElement();
        float newFireRate = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower stats
            towerObj.setBaseFireRate(newFireRate);
            towerObj.setFireRateLevel(towerObj.getFireRateLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (towerObj.getFireRateLevel() + 1).ToString() + "_frCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Fire Rate: " + towerObj.getFireRate().ToString();
            highlightBars(row, towerObj.getFireRateLevel(), 5);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeRange(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (towerObj.getRangeLevel() + 1).ToString() + "_newRange_" + towerObj.getElement();
        float newRange = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower stats
            towerObj.setBaseRange(newRange);
            towerObj.setRangeLevel(towerObj.getRangeLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (towerObj.getRangeLevel() + 1).ToString() + "_rangeCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Range: " + towerObj.getRange().ToString();
            highlightBars(row, towerObj.getRangeLevel(), 5);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeHealAmount(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (heal.getHealAmountLevel() + 1).ToString() + "_newHeal_" + towerObj.getElement();
        float newHeal = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower heal stats
            heal.setBaseHealAmount(newHeal);
            heal.setHealLevel(heal.getHealAmountLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (heal.getHealAmountLevel() + 1).ToString() + "_healCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Heal Amount: " + heal.getHealAmount().ToString();
            highlightBars(row, heal.getHealAmountLevel(), 5);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeHealRate(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (heal.getHealRateLevel() + 1).ToString() + "_newHealRate_" + towerObj.getElement();
        float newHealRate = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower heal stats
            heal.setBaseHealRate(newHealRate);
            heal.setHealRateLevel(heal.getHealRateLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (heal.getHealRateLevel() + 1).ToString() + "_healRateCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Heal Rate: " + heal.getHealRate().ToString();
            highlightBars(row, heal.getHealRateLevel(), 5);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeSlowPercent(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (slow.getSlowPercentLevel() + 1).ToString() + "_newSlowPercent_" + towerObj.getElement();
        float newSlowPercent = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower slow stats
            slow.setSlowPercent(newSlowPercent);
            slow.setSlowPercentLevel(slow.getSlowPercentLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (slow.getSlowPercentLevel() + 1).ToString() + "_slowPercentCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Slow Percent: " + slow.getSlowPercent().ToString();
            highlightBars(row, slow.getSlowPercentLevel(), 3);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeSlowDuration(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (slow.getSlowDurationLevel() + 1).ToString() + "_newSlowDur_" + towerObj.getElement();
        float newSlowDuration = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower slow stats
            slow.setSlowDuration(newSlowDuration);
            slow.setSlowDurationLevel(slow.getSlowDurationLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (slow.getSlowDurationLevel() + 1).ToString() + "_slowDurCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Slow Duration: " + slow.getSlowDuration().ToString();
            highlightBars(row, slow.getSlowDurationLevel(), 3);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    public void upgradeSpecial(int row, int cost, string upgradeName)
    {
        string valueName = "level_" + (towerObj.getSpecialLevel() + 1).ToString() + "_newSpecial_" + towerObj.getElement();
        float newSpecial = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            // Update tower stats
            towerObj.setSpecial(newSpecial);
            towerObj.setSpecialLevel(towerObj.getSpecialLevel() + 1);
            buildController.disablePoorText();

            // Update tower upgrade screen UI
            costName = "level_" + (towerObj.getSpecialLevel() + 1).ToString() + "_specialCost";
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = "Special: " + towerObj.getSpecial().ToString();
            highlightBars(row, towerObj.getSpecialLevel(), 3);
            setButtons(row, towerUpgrades.getCost(costName), upgradeName);
        }
        else
        {
            buildController.showPoorText();
        }
    }

    // Methods
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

                    setBars((i + 1), 5);
                    highlightBars(i, towerObj.getDMGLevel(), 5);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "firerate":
                    costName = "level_" + (towerObj.getFireRateLevel() + 1).ToString() + "_frCost";
                    rowText.text = "Fire Rate: " + towerObj.getFireRate().ToString();
                    rowDesc.text = "How fast the tower attacks";

                    setBars((i + 1), 5);
                    highlightBars(i, towerObj.getFireRateLevel(), 5);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "range":
                    costName = "level_" + (towerObj.getRangeLevel() + 1).ToString() + "_rangeCost";
                    rowText.text = "Range: " + towerObj.getRange().ToString();
                    rowDesc.text = "How far the tower can attack";

                    setBars((i + 1), 5);
                    highlightBars(i, towerObj.getRangeLevel(), 5);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "slowPercent":
                    slow = selectedTower.GetComponent<Slow>();

                    costName = "level_" + (slow.getSlowPercentLevel() + 1).ToString() + "_slowPercentCost";
                    rowText.text = "Slow Percent: " + slow.getSlowPercent().ToString();
                    rowDesc.text = "How much the tower slows enemies on attack";

                    setBars((i + 1), 3);
                    highlightBars(i, slow.getSlowPercentLevel(), 3);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "slowDuration":
                    slow = selectedTower.GetComponent<Slow>();

                    costName = "level_" + (slow.getSlowDurationLevel() + 1).ToString() + "_slowDurCost";
                    rowText.text = "Slow Duration: " + slow.getSlowDuration().ToString();
                    rowDesc.text = "How long the enemies get slowed for";

                    setBars((i + 1), 3);
                    highlightBars(i, slow.getSlowDurationLevel(), 3);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "healAmount":
                    heal = selectedTower.GetComponent<Heal>();

                    costName = "level_" + (heal.getHealAmountLevel() + 1).ToString() + "_healCost";
                    rowText.text = "Heal Amount: " + heal.getHealAmount().ToString();
                    rowDesc.text = "How much the tower heals every pulse";

                    setBars((i + 1), 5);
                    highlightBars(i, heal.getHealAmountLevel(), 5);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "healRate":
                    heal = selectedTower.GetComponent<Heal>();

                    costName = "level_" + (heal.getHealRateLevel() + 1).ToString() + "_healRateCost";
                    rowText.text = "Heal Rate: " + heal.getHealRate().ToString();
                    rowDesc.text = "How many times the tower pulses per second";

                    setBars((i + 1), 5);
                    highlightBars(i, heal.getHealRateLevel(), 5);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                case "special":
                    costName = "level_" + (towerObj.getSpecialLevel() + 1).ToString() + "_specialCost";

                    if (towerObj.getSpecialLevel() == 0)
                    {
                        rowText.text = "Unlock Special";
                        rowDesc.text = towerObj.getSpecialDesc();
                    } else
                    {
                        rowText.text = "Special: " + towerObj.getSpecial().ToString();
                        rowDesc.text = towerObj.getSpecialUpgradeDesc();

                        highlightBars(i, towerObj.getSpecialLevel(), 3);
                    }

                    setBars((i + 1), 3);
                    setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);

                    break;

                default:
                    rowText.text = "";
                    rowDesc.text = "";

                    break;
            }
        }
    }

    public void highlightBars(int row, int currentLevel, int maxLevel)
    {
        switch (row)
        {
            case 0:
                for (int i = 0; i < maxLevel; i++)
                {
                    bar = upgradeBars1.GetChild(i).GetComponent<Image>();

                    if (i < currentLevel)
                        bar.color = upgradedColor;
                    else
                        bar.color = notUpgradedColor;
                }
                break;
            case 1:
                for (int i = 0; i < maxLevel; i++)
                {
                    bar = upgradeBars2.GetChild(i).GetComponent<Image>();

                    if (i < currentLevel)
                        bar.color = upgradedColor;
                    else
                        bar.color = notUpgradedColor;
                }
                break;
            case 2:
                for (int i = 0; i < maxLevel; i++)
                {
                    bar = upgradeBars3.GetChild(i).GetComponent<Image>();

                    if (i < currentLevel)
                        bar.color = upgradedColor;
                    else
                        bar.color = notUpgradedColor;
                }
                break;
            case 3:
                for (int i = 0; i < maxLevel; i++)
                {
                    bar = upgradeBars4.GetChild(i).GetComponent<Image>();

                    if (i < currentLevel)
                        bar.color = upgradedColor;
                    else
                        bar.color = notUpgradedColor;
                }
                break;
            default:
                break;
        }
    }

    public void setButtons(int row, int cost, string upgradeName)
    {
        button = buttonContainer.GetChild(row).GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        text = buttonContainer.GetChild(row).GetComponentInChildren<TMP_Text>();
        text.text = cost.ToString();

        // UnityEvent <= button.onClick // needs to be called on a Button component
        // button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(<function to run>);

        switch (upgradeName)
        {
            case "damage":
                button.onClick.AddListener(() => upgradeDamage(row, cost, upgradeName));
                break;

            case "firerate":
                button.onClick.AddListener(() => upgradeFireRate(row, cost, upgradeName));
                break;

            case "range":
                button.onClick.AddListener(() => upgradeRange(row, cost, upgradeName));
                break;

            case "healAmount":
                button.onClick.AddListener(() => upgradeHealAmount(row, cost, upgradeName));
                break;

            case "healRate":
                button.onClick.AddListener(() => upgradeHealRate(row, cost, upgradeName));
                break;

            case "slowPercent":
                button.onClick.AddListener(() => upgradeSlowPercent(row, cost, upgradeName));
                break;

            case "slowDuration":
                button.onClick.AddListener(() => upgradeSlowDuration(row, cost, upgradeName));
                break;

            case "special":
                button.onClick.AddListener(() => upgradeSpecial(row, cost, upgradeName));
                break;

            default:
                break;
        }
    }

    public void setBars(int barNum, int maxLevel)
    {
        Transform currentTransform;

        switch (barNum)
        {
            case 1:
                currentTransform = upgradeBars1;
                break;
            case 2:
                currentTransform = upgradeBars2;
                break;
            case 3:
                currentTransform = upgradeBars3;
                break;
            case 4:
                currentTransform = upgradeBars4;
                break;
            default:
                currentTransform = null;
                break;
        }

        for (int i = 0; i <= 4; i++)
        {
            if (i < maxLevel)
                currentTransform.GetChild(i).gameObject.SetActive(true);
            else
                currentTransform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void emptySelectedTower()
    {
        selectedTower = null;
    }

    public void setSelectedTower(GameObject obj)
    {
        selectedTower = obj;
        towerObj = selectedTower.GetComponent<TowerObject>();

        setUpgradeMenu();
    }

    // Animation methods
    public void setIsSelected(bool c)
    {
        upgradeMenuAnimation.SetBool("isSelected", c);
    }

    public void startTrigger(string triggerName)
    {
        upgradeMenuAnimation.SetTrigger(triggerName);
    }
}
