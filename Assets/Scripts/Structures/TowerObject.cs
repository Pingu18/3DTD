using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class TowerObject : MonoBehaviour, IDamageable
{
    [SerializeField] private TowerStats towerStats;
    private TowerBuffHandler buffHandler;

    // Script references
    private BuildController buildCon;
    private ActionDict actionDict;
    private ElementalSystem elementalSystem;
    private TowerAttack towerAttack;

    // Static stats
    private string towerName;
    private string element;

    // Base stats
    private float baseHP;
    private float baseDamage;
    private float baseFireRate;
    private float baseRange;

    // Current stats
    private float maxHP;
    private float currentHP;
    private float damage;
    private float fireRate;
    private float range;
    private float special;

    // Stat levels
    private int hpLvl;
    private int dmgLvl;
    private int fireRateLvl;
    private int rangeLvl;
    private int specialLvl;

    // Misc stats
    private float takeDamageMultiplier;
    private float damageMultiplier;
    private int resaleValue;
    private int cost;

    // Variables
    private float nextFire;
    GameObject currentTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>(); // list of enemies in radius
    private GameObject detectionRadius;
    private Vector3 detectionScale;
    private bool drawRadius = false;

    private Queue<DamageInfo> damageQueue = new Queue<DamageInfo>();

    private bool keepAttacking = true;

    private Slider healthBar;
    [SerializeField] private Image healthBarImage;
    private Color maxHPColor;
    private Color minHPColor;

    struct DamageInfo
    {
        public float damageTaken;
        public GameObject enemy;

        public DamageInfo(float dmgTaken, GameObject obj)
        {
            damageTaken = dmgTaken;
            enemy = obj;
        }
    }

    private void Start()
    {
        Debug.Log("Creating TowerObject... (TowerObject)");
        towerName = towerStats.towerName;
        element = towerStats.element;

        baseHP = towerStats.maxHealth;
        baseDamage = towerStats.damage;
        baseFireRate = towerStats.attackSpeed;
        baseRange = towerStats.range;

        maxHP = towerStats.maxHealth;
        currentHP = towerStats.maxHealth;
        damage = towerStats.damage;
        fireRate = towerStats.attackSpeed;
        range = towerStats.range;
        special = towerStats.special;

        hpLvl = 0;
        dmgLvl = 0;
        fireRateLvl = 0;
        rangeLvl = 0;
        specialLvl = 0;

        takeDamageMultiplier = towerStats.takeDamageMultiplier;
        damageMultiplier = towerStats.damageMultiplier;
        cost = towerStats.cost;
        resaleValue = (int)Mathf.Ceil(cost * 0.75f);

        buffHandler = GetComponent<TowerBuffHandler>();

        actionDict = transform.parent.parent.gameObject.GetComponent<ActionDict>();
        elementalSystem = transform.parent.parent.gameObject.GetComponent<ElementalSystem>();
        towerAttack = transform.parent.parent.gameObject.GetComponent<TowerAttack>();

        buildCon = FindObjectOfType<BuildController>();
        nextFire = 0.0f;
        detectionRadius = this.gameObject.transform.GetChild(0).gameObject;
        updateRangeCollider(range);

        healthBar = GetComponentInChildren<Slider>();
        maxHPColor = new Color(42f / 255f, 255f / 255f, 46f / 255f);
        minHPColor = new Color(255f / 255f, 87f / 255f, 61f / 255f);
        updateHealthBar();

        StartCoroutine(startAttackCycle());
    }

    private void Update()
    {
        if (damageQueue.Count != 0)
        {
            DamageInfo dInfo = damageQueue.Dequeue();

            if ((currentHP - dInfo.damageTaken) <= 0)
                damageQueue.Clear();

            takeDamage(dInfo.damageTaken, dInfo.enemy);
        }

        if (buildCon.getInBuild() && drawRadius)
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Damage functions
    public void queueDamage(float dmgTaken, GameObject enemy, bool playerDamage)
    {
        DamageInfo dInfo = new DamageInfo(dmgTaken, enemy);
        damageQueue.Enqueue(dInfo);
    }

    private void takeDamage(float dmgTaken, GameObject enemy)
    {
        if (enemy != null)
        {
            float elementalDamageMultiplier = elementalSystem.getElementalMultiplier(enemy.GetComponent<EnemyObject>().getElement(), element);

            currentHP -= dmgTaken * elementalDamageMultiplier;
            updateHealthBar();
            checkDeath(enemy);
        }
    }

    // Health functions
    private void updateHealthBar()
    {
        float hpPercent = getHealthPercent();

        healthBar.value = hpPercent;
        healthBarImage.color = Color.Lerp(minHPColor, maxHPColor, hpPercent / 100);
    }

    private float getHealthPercent()
    {
        return (currentHP / maxHP) * 100;
    }

    public void AddHP(float amount)
    {
        if (currentHP + amount > maxHP)
            currentHP = maxHP;
        else
            currentHP += amount;

        updateHealthBar();
    }

    public void healToFull()
    {
        currentHP = maxHP;
        updateHealthBar();
    }

    // Logic functions
    private void checkDeath(GameObject enemy)
    {
        if (currentHP <= 0)
        {
            if (enemy != null)
                enemy.GetComponent<EnemyObject>().removeTarget(this.gameObject);

            Destroy(transform.parent.gameObject);
        }
    }

    // Attack functions
    private IEnumerator startAttackCycle()
    {
        while (keepAttacking)
        {
            // Wait until there are targets in range and tower is ready to attack
            yield return StartCoroutine(readyToAttack());
            currentTarget = selectBestTarget();
            attackTarget(currentTarget);
        }
    }

    private IEnumerator readyToAttack()
    {
        yield return new WaitUntil(() => targets.Count > 0 && Time.time > nextFire);
    }

    public void AddTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
    }

    private void attackTarget(GameObject target)
    {
        if (target != null)
        {
            if (actionDict.getAttackFX(towerName) != null)
            {
                GameObject atk = Instantiate(actionDict.getAttackFX(towerName), target.transform.position, Quaternion.identity);

                towerAttack.generateAttack(atk, target, this, actionDict.getAttackName(towerName));
            }

            nextFire = Time.time + (1 / fireRate);
        }
    }

    private GameObject selectBestTarget()
    {
        float closestDist = 999;
        GameObject closestTarget = targets[0];

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                float targetDistFromGoal = targets[i].GetComponent<EnemyObject>().getDistFromGoal();
                if (targetDistFromGoal < closestDist)
                {
                    closestDist = targetDistFromGoal;
                    closestTarget = targets[i].gameObject;
                }
            }
        }
        return closestTarget;
    }

    // Methods
    public void increaseDamage(float percent)
    {
        // increase damage by percent
        damageMultiplier += percent;
    }

    public void reduceDamage(float percent)
    {
        damageMultiplier -= percent;
    }

    public void updateRangeCollider(float range)
    {
        detectionScale = new Vector3(range, range, range);
        SetGlobalScale(detectionRadius.transform, detectionScale);
    }

    private void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    public void addResaleValue(int toAdd)
    {
        resaleValue += toAdd;
    }

    // Setter methods
    public void setOutline(bool isSelected)
    {
        this.gameObject.GetComponent<Outline>().enabled = isSelected;
    }

    public void setBaseHP(int newHealth)
    {
        baseHP = newHealth;
        currentHP = newHealth;
        maxHP = newHealth;
    }

    public void setBaseDamage(int newDamage)
    {
        baseDamage = newDamage;
        damage = newDamage * damageMultiplier;
    }

    public void setDamage(int newDamage)
    {
        damage = newDamage * damageMultiplier;
    }

    public void setBaseFireRate(float newFireRate)
    {
        baseFireRate = newFireRate;
        fireRate = newFireRate;
    }

    public void setFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void setBaseRange(float newRange)
    {
        baseRange = newRange;
        range = newRange;

        updateRangeCollider(newRange);
    }

    public void setRange(float newRange)
    {
        range = newRange;

        updateRangeCollider(newRange);
    }

    public void setSpecial(float newSpecial)
    {
        special = newSpecial;
    }

    public void setHPLevel(int newLvl)
    {
        hpLvl = newLvl;
    }

    public void setDMGLevel(int newLvl)
    {
        dmgLvl = newLvl;
    }

    public void setFireRateLevel(int newLvl)
    {
        fireRateLvl = newLvl;
    }

    public void setRangeLevel(int newLvl)
    {
        rangeLvl = newLvl;
    }

    public void setSpecialLevel(int newLvl)
    {
        specialLvl = newLvl;

        if (specialLvl > 0)
            buffHandler.enableSpecial(towerName);
    }

    // Getter methods
    public string getSpecialDesc()
    {
        return towerStats.specialDesc;
    }

    public string getSpecialUpgradeDesc()
    {
        return towerStats.specialUpgradeDesc;
    }

    public int getHPLevel()
    {
        return hpLvl;
    }

    public int getDMGLevel()
    {
        return dmgLvl;
    }

    public int getFireRateLevel()
    {
        return fireRateLvl;
    }

    public int getRangeLevel()
    {
        return rangeLvl;
    }

    public int getSpecialLevel()
    {
        return specialLvl;
    }

    public int getResaleValue()
    {
        return resaleValue;
    }

    public string getElement()
    {
        return element;
    }

    public string getName()
    {
        return towerName;
    }

    public float getMaxHP()
    {
        return maxHP;
    }

    public float getCurrentHP()
    {
        return currentHP;
    }

    public float getDamage()
    {
        return damage * damageMultiplier;
    }

    public float getBaseHP()
    {
        return baseHP;
    }

    public float getBaseDamage()
    {
        return baseDamage;
    }

    public float getBaseFireRate()
    {
        return baseFireRate;
    }

    public float getBaseRange()
    {
        return baseRange;
    }

    public float getFireRate()
    {
        return fireRate;
    }

    public float getRange()
    {
        return range;
    }

    public float getSpecial()
    {
        return special;
    }

    public int getCost()
    {
        return towerStats.cost;
    }

    public float getHeal()
    {
        return towerStats.heal;
    }

    public float getHealRate()
    {
        return towerStats.healRate;
    }

    public float getSlowPercent()
    {
        return towerStats.slowPercent;
    }

    public float getSlowDuration()
    {
        return towerStats.slowDuration;
    }

    public string[] getUpgrades()
    {
        return towerStats.upgrades;
    }

    public float getDamageMultiplier()
    {
        return damageMultiplier;
    }
}
