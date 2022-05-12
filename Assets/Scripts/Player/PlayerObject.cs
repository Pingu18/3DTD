using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerObject : MonoBehaviour
{
    private PlayerController playerController;
    private SkillDict skillDict;
    private BuildController buildCon;
    private Teleport teleport;

    [SerializeField] private TimerUI timerUI;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private string element;

    [SerializeField] private List<GameObject> skills;
    [SerializeField] private List<float> cooldowns;
    [SerializeField] private List<float> timers;

    private void Start()
    {
        Debug.Log("Starting PlayerObject...");

        skillDict = GetComponent<SkillDict>();
        playerController = GetComponent<PlayerController>();
        buildCon = FindObjectOfType<BuildController>();
        teleport = FindObjectOfType<Teleport>();

        timers.Add(0f);
        timers.Add(0f);
        timers.Add(0f);

        element = playerController.getElement();
        grabSkills();
    }

    private void Update()
    {
        if (!buildCon.getInBuild() && !teleport.inTeleport)
            checkSkill();
    }

    private void checkSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > timers[0])
        {
            startCooldown(0);
            startSkill(0);
        } else if (Input.GetKeyDown(KeyCode.E) && Time.time > timers[1])
        {
            startCooldown(1);
            startSkill(1);
        } else if (Input.GetKeyDown(KeyCode.R) && Time.time > timers[2])
        {
            startCooldown(2);
            startSkill(2);
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
        skill.GetComponent<SphereCollider>().enabled = true;
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
}
