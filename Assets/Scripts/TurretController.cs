using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] TurretAssembler turretAssembler;

    [SerializeField] public List<GameObject> enemies = new List<GameObject>();

    [Header("[STATS]")]
    public float bulletSpeed;
    public float range;
    public float fireRate;
    public float damage;
    public float elementMultiplier;
    public string extraEffect;

    [Header("[COMPONENTS]")]
    [SerializeField] GameObject shootPoint;
    [SerializeField] GameObject turretTop;
    [SerializeField] GameObject canon;
    [SerializeField] GameObject rangeSphere;
    public string bulletType;
    [SerializeField] SphereCollider rangeCol;

    [SerializeField] ParticleSystem shootMuzzle;

    [SerializeField] ObjectPooler pooler;

    float lookingAngle;
    bool canShoot;
    private void OnEnable()
    {
        canShoot = true;
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
        canShoot = false;

        LeanTween.moveLocalY(canon, -0.025f, 0.1f).setLoopPingPong(1);
        shootMuzzle.Play();

        GameObject bullet = pooler.SpawnFromPool($"{bulletType}", shootPoint.transform.position, shootPoint.transform.rotation);
        Bullet bulletCs = bullet.GetComponent<Bullet>();
        bulletCs.damage = damage;

        Vector3 direction = (enemies[0].transform.position - gameObject.transform.position).normalized;

        bulletCs.tweenID = LeanTween.move(bullet, shootPoint.transform.position + (direction * range), 1f / bulletSpeed).setOnComplete(() =>
        {
            bulletCs.DisableBullet();
        }).uniqueId;

        LeanTween.delayedCall((1f / fireRate), () =>
        {
            canShoot = true;
        });
    }

    public void UpdateRangeSphere()
    {
        rangeSphere.transform.localScale = new Vector3(range, range, range);
        rangeCol.radius = range / 2;
    }

    public void UpdateBulletType()
    {
        
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
