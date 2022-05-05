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

    // Tower Abilites (excluding Specials)
    private Heal heal;
    private Slow slow;
    private Bounce bounce;

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
    private int maxLevel;

    //[SerializeField] private <Name of buff/debuff script here>;

    [Header("Screens")]
    [SerializeField] private GameObject statScreen;
    [SerializeField] private GameObject upgradeScreen;

    [Header("Screen Buttons")]
    [SerializeField] private Button statButton;
    [SerializeField] private Button upgradeButton;

    [Header("Menu Stats")]
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerElementText;

    [Header("Stat Menu References")]
    [SerializeField] private TMP_Text baseHPStats;
    [SerializeField] private TMP_Text hpStats;
    [SerializeField] private TMP_Text baseDamageStats;
    [SerializeField] private TMP_Text damageStats;
    [SerializeField] private TMP_Text baseFireRateStats;
    [SerializeField] private TMP_Text fireRateStats;
    [SerializeField] private TMP_Text baseRangeStats;
    [SerializeField] private TMP_Text rangeStats;

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
    public void upgrade(int row, int level, int maxLevel, int cost, string upgradeName, string dictValueName, string dictCostName)
    {
        string valueName = "level_" + (level + 1).ToString() + dictValueName + towerObj.getElement();
        float newValue = towerUpgrades.getValue(valueName);
        string costName;

        if (currencyController.checkSufficientMoney(cost))
        {
            switch (upgradeName)
            {
                case "Health":
                    towerObj.setBaseHP((int)newValue);
                    towerObj.setHPLevel(level + 1);
                    break;
                case "Damage":
                    towerObj.setBaseDamage((int)newValue);
                    towerObj.setDMGLevel(level + 1);
                    break;
                case "Fire Rate":
                    towerObj.setBaseFireRate(newValue);
                    towerObj.setFireRateLevel(level + 1);
                    break;
                case "Range":
                    towerObj.setBaseRange(newValue);
                    towerObj.setRangeLevel(level + 1);
                    break;
                case "Heal Amount":
                    heal.setBaseHealAmount(newValue);
                    heal.setHealLevel(level + 1);
                    break;
                case "Heal Rate":
                    heal.setBaseHealRate(newValue);
                    heal.setHealRateLevel(level + 1);
                    break;
                case "Slow Percent":
                    slow.setSlowPercent(newValue);
                    slow.setSlowPercentLevel(level + 1);
                    break;
                case "Slow Duration":
                    slow.setSlowDuration(newValue);
                    slow.setSlowDurationLevel(level + 1);
                    break;
                case "Bounces":
                    bounce.setMaxBounces((int)newValue);
                    bounce.setBounceLevel(level + 1);
                    break;
                case "Special":
                    towerObj.setSpecial(newValue);
                    towerObj.setSpecialLevel(level + 1);
                    rowDescs.GetChild(row).GetComponent<TMP_Text>().text = towerObj.getSpecialUpgradeDesc();
                    updateSpecial();
                    break;
            }

            buildController.disablePoorText();
            towerObj.addResaleValue((int)Mathf.Ceil(cost * 0.75f));

            costName = "level_" + (level + 2).ToString() + dictCostName;
            rowTexts.GetChild(row).GetComponent<TMP_Text>().text = upgradeName + ": " + newValue.ToString();
            highlightBars(row, (level + 1), maxLevel);
            setStatMenu();

            if (level + 2 <= maxLevel)
                setButtons(row, towerUpgrades.getCost(costName), upgradeName);
            else
                setMaxButtons(row);

        }
        else
        {
            buildController.showPoorText();
        }
    }

    // Methods
    public void updateSpecial()
    {
        switch (towerObj.getName())
        {
            case "Water Tower":
                towerObj.GetComponent<Stun>().updateDuration(towerObj.getSpecial());
                break;
            case "Lightning Tower":
                towerObj.GetComponent<Stun>().updateDuration(towerObj.getSpecial());
                break;
            case "Grass Tower":
                towerObj.GetComponent<Lifesteal>().updateLifestealAmount(towerObj.getSpecial());
                break;
        }
    }

    public void setStatMenu()
    {
        towerNameText.text = towerObj != null ? towerObj.getName() : "";
        towerElementText.text = towerObj != null ? "Element: " + towerObj.getElement() : "";

        baseHPStats.text = "Base HP: " + towerObj.getBaseHP();
        hpStats.text = "HP: " + towerObj.getCurrentHP() + " / " + towerObj.getMaxHP();

        baseDamageStats.text = "Base Damage: " + towerObj.getBaseDamage();
        damageStats.text = "Damage: " + towerObj.getDamage();

        baseFireRateStats.text = "Base Fire Rate: " + towerObj.getBaseFireRate();
        fireRateStats.text = "Fire Rate: " + towerObj.getFireRate();

        baseRangeStats.text = "Base Range: " + towerObj.getBaseRange();
        rangeStats.text = "Range: " + towerObj.getRange();

        // Create new script for each Tower to hold buff/debuffs?
        // Code to display those buff/debuffs from that script here...
    }

    public void setUpgradeMenu()
    {
        string[] upgrades = towerObj.getUpgrades();
        int numUpgrades = upgrades.Length;
        string costName;

        for (int i = 0; i < numUpgrades; i++)
        {
            TMP_Text rowText = rowTexts.GetChild(i).GetComponent<TMP_Text>();
            TMP_Text rowDesc = rowDescs.GetChild(i).GetComponent<TMP_Text>();

            switch (upgrades[i])
            {
                case "Health":
                    costName = "level_" + (towerObj.getHPLevel() + 1).ToString() + "_hpCost";
                    rowText.text = "Health: " + towerObj.getMaxHP().ToString();
                    rowDesc.text = "How much health the tower has";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, towerObj.getHPLevel(), maxLevel);

                    if (towerObj.getHPLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Damage":
                    costName = "level_" + (towerObj.getDMGLevel() + 1).ToString() + "_dmgCost";
                    rowText.text = "Damage: " + towerObj.getDamage().ToString();
                    rowDesc.text = "How much damage the tower deals";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, towerObj.getDMGLevel(), maxLevel);

                    if (towerObj.getDMGLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Fire Rate":
                    costName = "level_" + (towerObj.getFireRateLevel() + 1).ToString() + "_frCost";
                    rowText.text = "Fire Rate: " + towerObj.getFireRate().ToString();
                    rowDesc.text = "How fast the tower attacks";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, towerObj.getFireRateLevel(), maxLevel);

                    if (towerObj.getFireRateLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Range":
                    costName = "level_" + (towerObj.getRangeLevel() + 1).ToString() + "_rangeCost";
                    rowText.text = "Range: " + towerObj.getRange().ToString();
                    rowDesc.text = "How far the tower can attack";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, towerObj.getRangeLevel(), maxLevel);
                    
                    if (towerObj.getRangeLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Slow Percent":
                    slow = selectedTower.GetComponent<Slow>();

                    costName = "level_" + (slow.getSlowPercentLevel() + 1).ToString() + "_slowPercentCost";
                    rowText.text = "Slow Percent: " + slow.getSlowPercent().ToString();
                    rowDesc.text = "How much the tower slows enemies on attack";
                    maxLevel = 3;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, slow.getSlowPercentLevel(), maxLevel);
                    
                    if (slow.getSlowPercentLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Slow Duration":
                    slow = selectedTower.GetComponent<Slow>();

                    costName = "level_" + (slow.getSlowDurationLevel() + 1).ToString() + "_slowDurCost";
                    rowText.text = "Slow Duration: " + slow.getSlowDuration().ToString();
                    rowDesc.text = "How long the enemies get slowed for";
                    maxLevel = 3;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, slow.getSlowDurationLevel(), maxLevel);
                    
                    if (slow.getSlowDurationLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Heal Amount":
                    heal = selectedTower.GetComponent<Heal>();

                    costName = "level_" + (heal.getHealAmountLevel() + 1).ToString() + "_healCost";
                    rowText.text = "Heal Amount: " + heal.getHealAmount().ToString();
                    rowDesc.text = "How much the tower heals every pulse";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, heal.getHealAmountLevel(), maxLevel);
                    
                    if (heal.getHealAmountLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Heal Rate":
                    heal = selectedTower.GetComponent<Heal>();

                    costName = "level_" + (heal.getHealRateLevel() + 1).ToString() + "_healRateCost";
                    rowText.text = "Heal Rate: " + heal.getHealRate().ToString();
                    rowDesc.text = "How many times the tower pulses per second";
                    maxLevel = 5;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, heal.getHealRateLevel(), maxLevel);
                    
                    if (heal.getHealRateLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Bounces":
                    bounce = selectedTower.GetComponent<Bounce>();

                    costName = "level_" + (bounce.getBounceLevel() + 1).ToString() + "_bounceCost";
                    rowText.text = "Bounce: " + bounce.getMaxBounces().ToString();
                    rowDesc.text = "How many times the tower attack bounces";
                    maxLevel = 3;

                    setBars((i + 1), maxLevel);
                    highlightBars(i, bounce.getBounceLevel(), maxLevel);

                    if (bounce.getBounceLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

                    break;

                case "Special":
                    costName = "level_" + (towerObj.getSpecialLevel() + 1).ToString() + "_specialCost";
                    maxLevel = 3;

                    if (towerObj.getSpecialLevel() == 0)
                    {
                        rowText.text = "Unlock Special";
                        rowDesc.text = towerObj.getSpecialDesc();
                    } else
                    {
                        rowText.text = "Special: " + towerObj.getSpecial().ToString();
                        rowDesc.text = towerObj.getSpecialUpgradeDesc();
                    }

                    setBars((i + 1), maxLevel);
                    highlightBars(i, towerObj.getSpecialLevel(), maxLevel);

                    if (towerObj.getSpecialLevel() + 1 <= maxLevel)
                        setButtons(i, towerUpgrades.getCost(costName), upgrades[i]);
                    else
                        setMaxButtons(i);

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

    public void setMaxButtons(int row)
    {
        button = buttonContainer.GetChild(row).GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        text = buttonContainer.GetChild(row).GetComponentInChildren<TMP_Text>();
        text.text = "MAX";
    }

    public void setButtons(int row, int cost, string upgradeName)
    {
        button = buttonContainer.GetChild(row).GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        text = buttonContainer.GetChild(row).GetComponentInChildren<TMP_Text>();
        text.text = cost.ToString();

        switch (upgradeName)
        {
            case "Health":
                button.onClick.AddListener(() => upgrade(row, towerObj.getHPLevel(), 5, cost, upgradeName, "_newHP_", "_hpCost"));
                break;

            case "Damage":
                button.onClick.AddListener(() => upgrade(row, towerObj.getDMGLevel(), 5, cost, upgradeName, "_newDmg_", "_dmgCost"));
                break;

            case "Fire Rate":
                button.onClick.AddListener(() => upgrade(row, towerObj.getFireRateLevel(), 5, cost, upgradeName, "_newFr_", "_frCost"));
                break;

            case "Range":
                button.onClick.AddListener(() => upgrade(row, towerObj.getRangeLevel(), 5, cost, upgradeName, "_newRange_", "_rangeCost"));
                break;

            case "Heal Amount":
                button.onClick.AddListener(() => upgrade(row, heal.getHealAmountLevel(), 5, cost, upgradeName, "_newHeal_", "_healCost"));
                break;

            case "Heal Rate":
                button.onClick.AddListener(() => upgrade(row, heal.getHealRateLevel(), 5, cost, upgradeName, "_newHealRate_", "_healRateCost"));
                break;

            case "Slow Percent":
                button.onClick.AddListener(() => upgrade(row, slow.getSlowPercentLevel(), 3, cost, upgradeName, "_newSlowPercent_", "_slowPercentCost"));
                break;

            case "Slow Duration":
                button.onClick.AddListener(() => upgrade(row, slow.getSlowDurationLevel(), 3, cost, upgradeName, "_newSlowDur_", "_slowDurCost"));
                break;

            case "Bounces":
                button.onClick.AddListener(() => upgrade(row, bounce.getBounceLevel(), 3, cost, upgradeName, "_newBounce_", "_bounceCost"));
                break;

            case "Special":
                button.onClick.AddListener(() => upgrade(row, towerObj.getSpecialLevel(), 3, cost, upgradeName, "_newSpecial_", "_specialCost"));
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

        setStatMenu();
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
