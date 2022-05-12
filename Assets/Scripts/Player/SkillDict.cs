using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDict : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> skillsDict = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, List<float>> cooldownDict = new Dictionary<string, List<float>>();

    private List<GameObject> fireSkills = new List<GameObject>();
    private List<float> fireCD = new List<float>();

    [Header("Fire Skills")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionCD;

    private void Start()
    {
        Debug.Log("Starting SkillDict...");

        fireSkills.Add(explosion);
        fireCD.Add(5.0f);
        fireSkills.Add(explosion);
        fireCD.Add(10.0f);
        fireSkills.Add(explosion);
        fireCD.Add(5.0f);

        skillsDict.Add("Fire", fireSkills);
        cooldownDict.Add("Fire", fireCD);
    }

    public List<GameObject> getSkillList(string element)
    {
        return skillsDict[element];
    }

    public List<float> getSkillCooldown(string element)
    {
        return cooldownDict[element];
    }
}
