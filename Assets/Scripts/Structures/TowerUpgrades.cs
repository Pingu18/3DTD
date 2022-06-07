using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{
    Dictionary<string, int> costDict = new Dictionary<string, int>();
    Dictionary<string, float> valueDict = new Dictionary<string, float>();

    private void Start()
    {
        // HP
        // Earth
        costDict.Add("level_1_hpCost", 200);
        costDict.Add("level_2_hpCost", 600);
        costDict.Add("level_3_hpCost", 1000);
        costDict.Add("level_4_hpCost", 1500);
        costDict.Add("level_5_hpCost", 2500);

        valueDict.Add("level_1_newHP_Earth", 225);
        valueDict.Add("level_2_newHP_Earth", 250);
        valueDict.Add("level_3_newHP_Earth", 300);
        valueDict.Add("level_4_newHP_Earth", 400);
        valueDict.Add("level_5_newHP_Earth", 500);

        // DAMAGE
        // Fire | Earth | Lightning | Dark | Light
        costDict.Add("level_1_dmgCost", 400);
        costDict.Add("level_2_dmgCost", 800);
        costDict.Add("level_3_dmgCost", 1500);
        costDict.Add("level_4_dmgCost", 3000);
        costDict.Add("level_5_dmgCost", 6000);

        valueDict.Add("level_1_newDmg_Fire", 65);
        valueDict.Add("level_2_newDmg_Fire", 80);
        valueDict.Add("level_3_newDmg_Fire", 95);
        valueDict.Add("level_4_newDmg_Fire", 120);
        valueDict.Add("level_5_newDmg_Fire", 150);

        valueDict.Add("level_1_newDmg_Earth", 160);
        valueDict.Add("level_2_newDmg_Earth", 170);
        valueDict.Add("level_3_newDmg_Earth", 185);
        valueDict.Add("level_4_newDmg_Earth", 200);
        valueDict.Add("level_5_newDmg_Earth", 225);

        valueDict.Add("level_1_newDmg_Lightning", 30);
        valueDict.Add("level_2_newDmg_Lightning", 35);
        valueDict.Add("level_3_newDmg_Lightning", 40);
        valueDict.Add("level_4_newDmg_Lightning", 50);
        valueDict.Add("level_5_newDmg_Lightning", 60);

        valueDict.Add("level_1_newDmg_Dark", 25);
        valueDict.Add("level_2_newDmg_Dark", 30);
        valueDict.Add("level_3_newDmg_Dark", 35);
        valueDict.Add("level_4_newDmg_Dark", 45);
        valueDict.Add("level_5_newDmg_Dark", 60);

        valueDict.Add("level_1_newDmg_Light", 25);
        valueDict.Add("level_2_newDmg_Light", 30);
        valueDict.Add("level_3_newDmg_Light", 35);
        valueDict.Add("level_4_newDmg_Light", 45);
        valueDict.Add("level_5_newDmg_Light", 60);

        // FIRE RATE
        // Fire | Lightning | Dark | Light
        costDict.Add("level_1_frCost", 200);
        costDict.Add("level_2_frCost", 800);
        costDict.Add("level_3_frCost", 1500);
        costDict.Add("level_4_frCost", 3000);
        costDict.Add("level_5_frCost", 5000);

        valueDict.Add("level_1_newFr_Fire", 1.1f);
        valueDict.Add("level_2_newFr_Fire", 1.2f);
        valueDict.Add("level_3_newFr_Fire", 1.3f);
        valueDict.Add("level_4_newFr_Fire", 1.4f);
        valueDict.Add("level_5_newFr_Fire", 1.5f);

        valueDict.Add("level_1_newFr_Lightning", 1.65f);
        valueDict.Add("level_2_newFr_Lightning", 1.8f);
        valueDict.Add("level_3_newFr_Lightning", 1.95f);
        valueDict.Add("level_4_newFr_Lightning", 2.10f);
        valueDict.Add("level_5_newFr_Lightning", 2.25f);

        valueDict.Add("level_1_newFr_Dark", 1.1f);
        valueDict.Add("level_2_newFr_Dark", 1.2f);
        valueDict.Add("level_3_newFr_Dark", 1.3f);
        valueDict.Add("level_4_newFr_Dark", 1.4f);
        valueDict.Add("level_5_newFr_Dark", 1.5f);

        valueDict.Add("level_1_newFr_Light", 1.1f);
        valueDict.Add("level_2_newFr_Light", 1.2f);
        valueDict.Add("level_3_newFr_Light", 1.3f);
        valueDict.Add("level_4_newFr_Light", 1.4f);
        valueDict.Add("level_5_newFr_Light", 1.5f);

        // RANGE
        // Fire | Water | Grass | Dark | Light
        costDict.Add("level_1_rangeCost", 300);
        costDict.Add("level_2_rangeCost", 700);
        costDict.Add("level_3_rangeCost", 1300);
        costDict.Add("level_4_rangeCost", 2100);
        costDict.Add("level_5_rangeCost", 3100);

        valueDict.Add("level_1_newRange_Fire", 45f);
        valueDict.Add("level_2_newRange_Fire", 50f);
        valueDict.Add("level_3_newRange_Fire", 55f);
        valueDict.Add("level_4_newRange_Fire", 60f);
        valueDict.Add("level_5_newRange_Fire", 65f);

        valueDict.Add("level_1_newRange_Water", 45f);
        valueDict.Add("level_2_newRange_Water", 50f);
        valueDict.Add("level_3_newRange_Water", 55f);
        valueDict.Add("level_4_newRange_Water", 60f);
        valueDict.Add("level_5_newRange_Water", 65f);

        valueDict.Add("level_1_newRange_Grass", 45f);
        valueDict.Add("level_2_newRange_Grass", 50f);
        valueDict.Add("level_3_newRange_Grass", 55f);
        valueDict.Add("level_4_newRange_Grass", 60f);
        valueDict.Add("level_5_newRange_Grass", 65f);

        valueDict.Add("level_1_newRange_Dark", 55f);
        valueDict.Add("level_2_newRange_Dark", 60f);
        valueDict.Add("level_3_newRange_Dark", 65f);
        valueDict.Add("level_4_newRange_Dark", 70f);
        valueDict.Add("level_5_newRange_Dark", 75f);

        valueDict.Add("level_1_newRange_Light", 55f);
        valueDict.Add("level_2_newRange_Light", 60f);
        valueDict.Add("level_3_newRange_Light", 65f);
        valueDict.Add("level_4_newRange_Light", 70f);
        valueDict.Add("level_5_newRange_Light", 75f);

        // HEAL AMOUNT
        // Grass
        costDict.Add("level_1_healCost", 500);
        costDict.Add("level_2_healCost", 1000);
        costDict.Add("level_3_healCost", 4000);
        costDict.Add("level_4_healCost", 8000);
        costDict.Add("level_5_healCost", 15000);

        valueDict.Add("level_1_newHeal_Grass", 30);
        valueDict.Add("level_2_newHeal_Grass", 40);
        valueDict.Add("level_3_newHeal_Grass", 70);
        valueDict.Add("level_4_newHeal_Grass", 100);
        valueDict.Add("level_5_newHeal_Grass", 150);

        // HEAL RATE
        // Grass
        costDict.Add("level_1_healRateCost", 500);
        costDict.Add("level_2_healRateCost", 1000);
        costDict.Add("level_3_healRateCost", 2000);
        costDict.Add("level_4_healRateCost", 3500);
        costDict.Add("level_5_healRateCost", 5000);

        valueDict.Add("level_1_newHealRate_Grass", 0.35f);
        valueDict.Add("level_2_newHealRate_Grass", 0.45f);
        valueDict.Add("level_3_newHealRate_Grass", 0.55f);
        valueDict.Add("level_4_newHealRate_Grass", 0.65f);
        valueDict.Add("level_5_newHealRate_Grass", 0.75f);

        // SLOW PERCENT
        // Water | Earth
        costDict.Add("level_1_slowPercentCost", 1250);
        costDict.Add("level_2_slowPercentCost", 3500);
        costDict.Add("level_3_slowPercentCost", 7000);

        valueDict.Add("level_1_newSlowPercent_Water", 30);
        valueDict.Add("level_2_newSlowPercent_Water", 40);
        valueDict.Add("level_3_newSlowPercent_Water", 50);

        valueDict.Add("level_1_newSlowPercent_Earth", 60);
        valueDict.Add("level_2_newSlowPercent_Earth", 70);
        valueDict.Add("level_3_newSlowPercent_Earth", 80);

        // SLOW DURATION
        // Water
        costDict.Add("level_1_slowDurCost", 1000);
        costDict.Add("level_2_slowDurCost", 3000);
        costDict.Add("level_3_slowDurCost", 6000);

        valueDict.Add("level_1_newSlowDur_Water", 0.75f);
        valueDict.Add("level_2_newSlowDur_Water", 1f);
        valueDict.Add("level_3_newSlowDur_Water", 1.25f);

        // BOUNCES
        // Lightning
        costDict.Add("level_1_bounceCost", 3000);
        costDict.Add("level_2_bounceCost", 8000);
        costDict.Add("level_3_bounceCost", 15000);

        valueDict.Add("level_1_newBounce_Lightning", 3);
        valueDict.Add("level_2_newBounce_Lightning", 4);
        valueDict.Add("level_3_newBounce_Lightning", 5);

        // SPECIAL
        costDict.Add("level_1_specialCost", 8000);
        costDict.Add("level_2_specialCost", 15000);
        costDict.Add("level_3_specialCost", 25000);

        // Fire | Water | Grass | Earth | Lightning | Dark | Light
        valueDict.Add("level_1_newSpecial_Fire", 1f);
        valueDict.Add("level_2_newSpecial_Fire", 2f);
        valueDict.Add("level_3_newSpecial_Fire", 3f);

        valueDict.Add("level_1_newSpecial_Water", 1f);
        valueDict.Add("level_2_newSpecial_Water", 1.5f);
        valueDict.Add("level_3_newSpecial_Water", 2f);

        valueDict.Add("level_1_newSpecial_Grass", 10f);
        valueDict.Add("level_2_newSpecial_Grass", 15f);
        valueDict.Add("level_3_newSpecial_Grass", 20f);

        valueDict.Add("level_1_newSpecial_Earth", 0.1f);    // haven't set up Earth tower special upgrade
        valueDict.Add("level_2_newSpecial_Earth", 0.15f);
        valueDict.Add("level_3_newSpecial_Earth", 0.2f);

        valueDict.Add("level_1_newSpecial_Lightning", 0.1f);
        valueDict.Add("level_2_newSpecial_Lightning", 0.15f);
        valueDict.Add("level_3_newSpecial_Lightning", 0.2f);

        valueDict.Add("level_1_newSpecial_Dark", 100f);
        valueDict.Add("level_2_newSpecial_Dark", 250f);
        valueDict.Add("level_3_newSpecial_Dark", 500f);

        valueDict.Add("level_1_newSpecial_Light", 100f);
        valueDict.Add("level_2_newSpecial_Light", 250f);
        valueDict.Add("level_3_newSpecial_Light", 500f);
    }

    public int getCost(string name)
    {
        return costDict[name];
    }

    public float getValue(string name)
    {
        return valueDict[name];
    }
}
