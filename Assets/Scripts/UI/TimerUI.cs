using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image skill1Image;
    [SerializeField] private Image skill2Image;
    [SerializeField] private Image skill3Image;
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
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(0);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(0);
                break;
            case 2:
                skillImage = skill2Image;
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(1);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(1);
                break;
            case 3:
                skillImage = skill3Image;
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(2);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(2);
                break;
        }

        while (Time.time <= skillTimer)
        {
            skillImage.fillAmount = 1 - (skillTimer - Time.time) / skillCD;
            yield return null;
        }
    }
}
