using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPoolableObject
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] SpriteRenderer model;
    [SerializeField] SphereCollider col;
    [SerializeField] ParticleSystem deathPS;
    [SerializeField] AoESphere aoESphere;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    [SerializeField] GameObject healthBarFill, healthBar, dmgWhite;

    private Transform originalPos;

    public bool alive;

    public ParticleSystem[] elementPS;

    Color whiteClear = new Color(1f, 1f, 1f, 0f);

    public float poisonDmg;
    bool poisoned;
    bool onFire;
    int enemyIndex;
    public void OnObjectSpawn()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameObject.transform.localScale = Vector3.one;
        alive = true;
        currentHealth = maxHealth;
        healthBarFill.GetComponent<Image>().fillAmount = 1;

        healthBar.SetActive(true);
        originalPos = transform;
        model.enabled = true;
        col.enabled = true;

        LeanTween.scaleY(gameObject, 1.1f, 0.5f).setLoopPingPong();
    }

    public void GetHit(float damage, Bullet bullet)
    {
        LeanTween.scale(gameObject, Vector3.one, 0f);

        LeanTween.color(dmgWhite, Color.white, 0.05f).setOnComplete(() =>
        {
            LeanTween.color(dmgWhite, whiteClear, 0.05f);
        });

        if (poisoned && bullet)
        {
            currentHealth -= (damage * (1f + GameManager.Instance.debuffData.poisonBonusDamage[bullet.elementTier]));
            poisonDmg += (damage * (GameManager.Instance.debuffData.poisonBonusDamage[bullet.elementTier]) * 2);
        }
        else
        {
            currentHealth -= damage;
        }

        healthBarFill.GetComponent<Image>().fillAmount = (1f / maxHealth) * currentHealth;
        LeanTween.scale(gameObject, gameObject.transform.localScale * 1.1f, 0.1f).setLoopPingPong(1);

        if (bullet) TriggerElementEffect(bullet);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void TriggerElementEffect(Bullet bullet)
    {
        ElementDataSO debuffDataSO = GameManager.Instance.debuffData;
        TurretController turret = bullet.originalParent.GetComponentInParent<TurretController>();

        switch (bullet.bulletElement)
        {
            //==FIRE==
            case Bullet.Element.fire:
                if (onFire)
                {
                    //nothing
                }
                else
                {
                    StartCoroutine(Burn(debuffDataSO.burnDamage[bullet.elementTier], debuffDataSO.burnTicks[bullet.elementTier], debuffDataSO.burnTickRate[bullet.elementTier]));
                    onFire = true;
                }
                break;

            //==STEEL==
            case Bullet.Element.steel:

                GetHit(debuffDataSO.bonusDamage[bullet.elementTier], null);

                break;

            //==POISON==
            case Bullet.Element.poison:
                if (poisoned)
                {
                    //nothing
                }
                else
                {
                    StartCoroutine(Poison());
                    poisoned = true;
                }
                break;

            //==LIGHTNING==
            case Bullet.Element.lightning:

                if (bullet.canShock)
                {
                    GetHit(bullet.damage * debuffDataSO.lightningDamage[bullet.elementTier], null);
                    aoESphere.Activate(bullet.damage * debuffDataSO.lightningDamage[bullet.elementTier], 1f);
                    elementPS[2].Play();
                }
                break;
        }
    }

    IEnumerator Burn(float burnDamage, float burnTicks, float burnTickRate)
    {
        elementPS[0].Play();
        for (int i = 0; i < burnTicks; i++)
        {
            if (alive) GetHit(burnDamage, null);

            if (i == burnTicks - 1)
            {
                elementPS[0].Stop();
                onFire = false;
            } 
            yield return new WaitForSeconds(burnTickRate);
        }
    }

    IEnumerator Poison()
    {
        elementPS[1].Play();
        for (int i = 0; i < 3f; i++)
        {
            if (i == 3f - 1)
            {
                poisoned = false;
                if (alive) GetHit(poisonDmg, null);
                poisonDmg = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AoESphere")
        {
            GetHit(other.gameObject.GetComponentInParent<AoESphere>().sphereDamage, null);
        }
    }

    public void Die()
    {
        enemySpawner.allEnemies.Remove(gameObject);
        alive = false;
        healthBar.SetActive(false);
        deathPS.Play();
        model.enabled = false;
        col.enabled = false;

        LeanTween.delayedCall(0.5f, () =>
        {
            gameObject.transform.localPosition = originalPos.position;
        });

        TurretController[] turretControllers = FindObjectsOfType<TurretController>();

        foreach (TurretController turret in turretControllers)
        {
            turret.enemies.Remove(gameObject);
        }

        foreach(ParticleSystem ps in elementPS)
        {
            ps.Stop();
        }
    }
}