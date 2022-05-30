using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerObject : MonoBehaviour
{
    private PlayerController playerController;
    private AudioController audioCon;
    private SkillDict skillDict;
    private BuildController buildCon;
    private Teleport teleport;
    private float currentMana;
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRegen;
    private float manaRegenTimer = 0.0f;
    private float manaRegenWaitTime = 1.5f;

    [SerializeField] private TimerUI timerUI;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private string element;

    [SerializeField] private List<GameObject> skills;
    [SerializeField] private List<float> cooldowns;
    [SerializeField] private List<float> timers;
    [SerializeField] private List<float> manaCosts;

    private void Start()
    {
        Debug.Log("Starting PlayerObject...");

        skillDict = GetComponent<SkillDict>();
        playerController = GetComponent<PlayerController>();
        buildCon = FindObjectOfType<BuildController>();
        teleport = FindObjectOfType<Teleport>();
        audioCon = FindObjectOfType<AudioController>();

        timers.Add(0f);
        timers.Add(0f);
        timers.Add(0f);

        element = playerController.getElement();
        grabSkills();

        currentMana = maxMana;
        timerUI.updateManaUI(currentMana, maxMana);
        timerUI.updateManaCostText(manaCosts[0], manaCosts[1], manaCosts[2]);
    }

    private void Update()
    {
        if (!buildCon.getInBuild() && !teleport.inTeleport)
            checkSkill();

        if (Time.time > manaRegenTimer)
            regenerateMana(manaRegen);
    }

    private void checkSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > timers[0] && currentMana >= manaCosts[0])
        {
            startCooldown(0);
            startSkill(0);
            useMana(manaCosts[0]);
        } else if (Input.GetKeyDown(KeyCode.E) && Time.time > timers[1] && currentMana >= manaCosts[1])
        {
            startCooldown(1);
            startSkill(1);
            useMana(manaCosts[1]);
        }
        else if (Input.GetKeyDown(KeyCode.R) && Time.time > timers[2] && currentMana >= manaCosts[2])
        {
            startCooldown(2);
            startSkill(2);
            useMana(manaCosts[2]);
        }
    }

    private void startCooldown(int skillNum)
    {
        timers[skillNum] = Time.time + cooldowns[skillNum];
        StartCoroutine(timerUI.startCooldown(skillNum + 1));
    }

    private void startSkill(int skillNum)
    {
        GameObject skill = Instantiate(skills[skillNum], this.transform.position, Quaternion.identity);
        skill.transform.parent = this.transform;
        StartCoroutine(PlaySkill(skill));
    }

    // need to change into a more general functionality...
    IEnumerator PlaySkill(GameObject skill)
    {
        playerAnim.SetTrigger("Skill1");
        yield return new WaitForSeconds(0.5f);
        skill.transform.GetChild(0).GetComponent<VisualEffect>().Play();
        audioCon.PlaySound("FireSkill1", this.gameObject);
        skill.GetComponent<Animator>().SetTrigger("Explode");
        Destroy(skill, 1f);
    }

    private void grabSkills()
    {
        skills = skillDict.getSkillList(element);
        cooldowns = skillDict.getSkillCooldown(element);
    }

    public void setElement(string newElement)
    {
        element = newElement;
    }

    public GameObject getSkill(int skillNum)
    {
        return skills[skillNum];
    }

    public float getCooldownTimer(int skillNum)
    {
        return timers[skillNum];
    }

    public float getCooldown(int skillNum)
    {
        return cooldowns[skillNum];
    }

    private void regenerateMana(float manaRegen)
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            timerUI.updateManaUI(currentMana, maxMana);
        }

        Mathf.Clamp(currentMana, 0, maxMana);
    }

    public void useMana(float manaUsed)
    {
        if (currentMana - manaUsed < 0)
            currentMana = 0;
        else
            currentMana -= manaUsed;

        timerUI.updateManaUI(currentMana, maxMana);
        manaRegenTimer = Time.time + manaRegenWaitTime;
    }

    public void addMana(float mana)
    {
        if (currentMana + mana > maxMana)
            currentMana = maxMana;
        else
            currentMana += mana;

        timerUI.updateManaUI(currentMana, maxMana);
    }

    public float getMana()
    {
        return currentMana;
    }

}
