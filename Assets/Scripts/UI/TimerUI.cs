using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image skill1Slot;
    [SerializeField] private Image skill2Slot;
    [SerializeField] private Image skill3Slot;
    [SerializeField] private Image skill1Image;
    [SerializeField] private Image skill2Image;
    [SerializeField] private Image skill3Image;
    [SerializeField] private TMP_Text skill1ManaText;
    [SerializeField] private TMP_Text skill2ManaText;
    [SerializeField] private TMP_Text skill3ManaText;
    [SerializeField] private GameObject player;
    [SerializeField] private Image TPSlot;
    [SerializeField] private Image TPImage;
    [SerializeField] private Image manaBar;
    [SerializeField] private TMP_Text manaText;

    public IEnumerator startCooldown(int skill)
    {
        float skillTimer = 0f;
        float skillCD = 0f;
        Image skillImage = null;
        Image skillSlot = null;
        Color originalColor;

        switch (skill)
        {
            case 1:
                skillImage = skill1Image;
                skillSlot = skill1Slot;
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(0);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(0);
                break;
            case 2:
                skillImage = skill2Image;
                skillSlot = skill2Slot;
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(1);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(1);
                break;
            case 3:
                skillImage = skill3Image;
                skillSlot = skill3Slot;
                skillTimer = player.GetComponent<PlayerObject>().getCooldownTimer(2);
                skillCD = player.GetComponent<PlayerObject>().getCooldown(2);
                break;
            case 4:
                skillImage = TPImage;
                skillSlot = TPSlot;
                skillTimer = player.GetComponent<Teleport>().teleportTimer;
                skillCD = player.GetComponent<Teleport>().teleportCD;
                break;
        }

        originalColor = skillSlot.color;

        while (Time.time <= skillTimer)
        {
            skillImage.fillAmount = 1 - (skillTimer - Time.time) / skillCD;
            var tempColor = skillSlot.color;
            tempColor.a = 0.3f;
            skillSlot.color = tempColor;
            yield return null;
        }
        skillSlot.color = originalColor;
    }

    public void updateManaUI(float mana, float maxMana)
    {
        manaBar.fillAmount = mana / maxMana;
        manaText.text = Mathf.Floor(mana).ToString();
    }

    public void updateManaCostText(float skill1mana, float skill2mana, float skill3mana)
    {
        skill1ManaText.text = skill1mana.ToString();
        skill2ManaText.text = skill2mana.ToString();
        skill3ManaText.text = skill3mana.ToString();
    }
}
