using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();

    [SerializeField] GameObject shootPoint;
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootRange;
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;

    [SerializeField] GameObject turretTop;
    [SerializeField] GameObject canon;
    [SerializeField] TurretPartsSO turretPart;
    [SerializeField] SpriteRenderer[] turretParts;
    [SerializeField] SpriteRenderer[] tierColorParts;

    [SerializeField] float lookingAngle;

    [SerializeField] ParticleSystem shootMuzzle;

    [SerializeField] ObjectPooler pooler;

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
        Debug.Log("Shoot");
        LeanTween.moveLocalY(canon, -0.025f, 0.1f).setLoopPingPong(1);
        shootMuzzle.Play();
        GameObject bullet = pooler.SpawnFromPool("BulletSimple", shootPoint.transform.position, shootPoint.transform.rotation);
        Bullet bulletCs = bullet.GetComponent<Bullet>();

        Vector3 direction = (enemies[0].transform.position - gameObject.transform.position).normalized;

        bulletCs.tweenID = LeanTween.move(bullet, shootPoint.transform.position + (direction * shootRange), 1f / bulletSpeed).setOnComplete(() =>
        {
            bulletCs.DisableBullet();
        }).uniqueId;

        LeanTween.delayedCall((1f / attackSpeed), () =>
        {
            canShoot = true;
        });
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
