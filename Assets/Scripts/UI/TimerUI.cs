using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image skill1Image;
    [SerializeField] private GameObject player;

    public IEnumerator startCooldown(int skill)
    {
        float skillTimer = 0f;
        float skillCD = 0f;
        Image skillImage = null;

        switch (skill)
        {
            case 1:
                skillImage = skill1Image;
                skillTimer = player.GetComponent<PlayerController>().primarySkillCDTimer;
                skillCD = player.GetComponent<PlayerController>().primarySkillCD;
                break;
        }

        while (Time.time <= skillTimer)
        {
            skillImage.fillAmount = 1 - (skillTimer - Time.time) / skillCD;
            yield return null;
        }
    }
}
