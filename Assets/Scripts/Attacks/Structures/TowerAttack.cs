using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TowerAttack : MonoBehaviour
{
    public void generateAttack(GameObject atk, GameObject target, TowerObject tower, string attackName)
    {
        TowerBuffHandler buffHandler = tower.gameObject.GetComponent<TowerBuffHandler>();

        switch (attackName)
        {
            case "Flame Attack":
                DamageRamp dRamp = tower.gameObject.GetComponent<DamageRamp>();

                if (tower.GetComponent<TowerObject>().getSpecialLevel() > 0)
                {
                    dRamp.setTarget(target);
                }

                atk.GetComponent<FlameAttack>().setParams(tower.gameObject, target, tower.getDamage());
                target.GetComponent<IDamageable>().queueDamage(tower.getDamage() * dRamp.getDamageMultiplier(), tower.gameObject, false);
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                Destroy(atk, 1.2f);
                break;
            case "Blast":
                atk.GetComponent<BlastAttack>().target = target;
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                target.GetComponent<IDamageable>().queueDamage(tower.getDamage(), tower.gameObject, false);
                tower.GetComponent<Slow>().applySlow(target);
                Destroy(atk, 1.0f);
                break;
            case "Chill":
                atk.GetComponent<ChillAttack>().setParentTower(tower.gameObject);
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                Destroy(atk, 1.2f);
                break;
            case "Zap":
                tower.gameObject.GetComponent<Stun>().addAttack();
                atk.GetComponent<ZapAttack>().parentTower = tower.gameObject;
                atk.GetComponent<ZapAttack>().BeginAttack(tower.gameObject, target, 3);
                break;
            case "Light Mark":
                atk.GetComponent<LightAttack>().target = target;
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                target.GetComponent<IDamageable>().queueDamage(tower.getDamage(), tower.gameObject, false);

                if (tower.GetComponent<TowerObject>().getSpecialLevel() > 0)
                {
                    target.GetComponent<EnemyBuffHandler>().applyLightMark(tower);
                }

                Destroy(atk, 1.0f);
                break;
            case "Dark Mark":
                atk.GetComponent<DarkAttack>().target = target;
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                target.GetComponent<IDamageable>().queueDamage(tower.getDamage(), tower.gameObject, false);

                if (tower.GetComponent<TowerObject>().getSpecialLevel() > 0)
                {
                    target.GetComponent<EnemyBuffHandler>().applyDarkMark(tower);
                }

                Destroy(atk, 1.0f);
                break;
        }
    }
}
