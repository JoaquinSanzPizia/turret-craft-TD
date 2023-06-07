using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] TurretAssembler turretAssembler;

    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] public GameObject currentTarget;

    public enum Element { fire, ice, poison, lightning, steel }
    public enum ExtraEffect { bonusShot, critChance, randomPierce }
    [Header("[STATS]")]
    public Element element;
    public ExtraEffect extraEffect;
    public float bulletSpeed;
    public float range;
    public float fireRate;
    public float damage;
    public float elementMultiplier;

    [Header("[COMPONENTS]")]
    public GameObject shootPoint;
    [SerializeField] GameObject turretTop;
    [SerializeField] GameObject canon;
    [SerializeField] GameObject rangeSphere;
    public string bulletType;
    [SerializeField] SphereCollider rangeCol;

    [SerializeField] ObjectPooler pooler;

    public Color bulletColor;
    public int uChasisTier;

    float lookingAngle;
    bool canShoot;
    bool canShock;
    private void OnEnable()
    {
        canShoot = true;
        if (element == Element.lightning)
        {
            canShock = true;
        }
        //rangeSphere.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (enemies.Count > 0)
        {
            Vector3 aimDirection = (enemies[0].transform.position - transform.position).normalized;
            lookingAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            turretTop.transform.eulerAngles = new Vector3(0f, 0f, lookingAngle - 90f);
        }

        if (canShoot && enemies.Count > 0)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        currentTarget = enemies[0];
        canShoot = false;

        LeanTween.moveLocalY(canon, -0.025f, 0.1f).setLoopPingPong(1);

        GameObject bullet = pooler.SpawnFromPool($"{bulletType}", shootPoint.transform.position, shootPoint.transform.rotation);

        Bullet bulletCs = bullet.GetComponent<Bullet>();

        bulletCs.damage = damage;

        LookForElementEffect(bulletCs);
        LookForExtraEffect(bulletCs);  

        Vector3 direction = (currentTarget.transform.position - gameObject.transform.position).normalized;

        bulletCs.tweenID = LeanTween.move(bullet, shootPoint.transform.position + (direction * range), 1f / bulletSpeed).setOnComplete(() =>
        {
            bulletCs.DisableBullet();
        }).uniqueId;

        LeanTween.delayedCall((1f / fireRate), () =>
        {
            canShoot = true;
        });
    }

    void ExtraShoot()
    {
        currentTarget = enemies[0];

        LeanTween.moveLocalY(canon, -0.025f, 0.1f).setOnComplete(() => 
        {
            LeanTween.moveLocalY(canon, 0f, 0.1f);
        });

        GameObject bullet = pooler.SpawnFromPool($"{bulletType}", shootPoint.transform.position, shootPoint.transform.rotation);

        Bullet bulletCs = bullet.GetComponent<Bullet>();
        bulletCs.damage = damage;

        LookForElementEffect(bulletCs);

        Vector3 direction = (currentTarget.transform.position - gameObject.transform.position).normalized;

        bulletCs.tweenID = LeanTween.move(bullet, shootPoint.transform.position + ((direction * range) / 2), 1f / bulletSpeed).setOnComplete(() =>
        {
            bulletCs.DisableBullet();
        }).uniqueId;
    }

    void LookForExtraEffect(Bullet bullet)
    {
        switch (extraEffect)
        {
            case ExtraEffect.bonusShot:
                int bonusShotInt = Random.Range(0, 100);
                if (bonusShotInt < GameManager.Instance.effectData.extraShotChance[uChasisTier])
                {
                    LeanTween.delayedCall(0.1f, () =>
                    {
                        ExtraShoot();
                    });
                }
                break;

            case ExtraEffect.critChance:
                int critInt = Random.Range(0, 100);
                if (critInt < GameManager.Instance.effectData.critChance[uChasisTier])
                {
                    bullet.damage = damage * 2f;
                }
                break;

            case ExtraEffect.randomPierce:
                int pierceInt = Random.Range(0, 100);
                if (pierceInt < GameManager.Instance.effectData.pierceChance[uChasisTier])
                {
                    bullet.temporaryPierces = true;
                }
                break;
        }
    }

    void LookForElementEffect(Bullet bullet)
    {
        switch(element)
        {
            case Element.lightning:
                if (canShock)
                {
                    canShock = false;
                    bullet.canShock = true;
                    bullet.effectParticles[0].Play();
                    Debug.Log("Shooting Lighning");

                    StartCoroutine(LightningCooldown());
                }
                break;
        }
    }

    IEnumerator LightningCooldown()
    {
        yield return new WaitForSeconds(GameManager.Instance.debuffData.lightningCooldown[uChasisTier]);

        canShock = true;
        Debug.Log("Lightning Ready");
    }

    public void UpdateRangeSphere()
    {
        rangeSphere.transform.localScale = new Vector3(range, range, range);
        rangeCol.radius = range / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }
}
