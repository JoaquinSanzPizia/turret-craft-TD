using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] ParticleSystem shootMuzzle;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject shootPoint;
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootRange;
    [SerializeField] float attackSpeed;
    [SerializeField] int damage;

    public GameObject canon;
    [SerializeField] float lookingAngle;
    [SerializeField] bool isCanon;

    bool canShoot;
    private void OnEnable()
    {
        canShoot = true;
    }
    private void FixedUpdate()
    {
        Vector3 aimDirection = (enemy.transform.position - transform.position).normalized;
        lookingAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        canon.transform.eulerAngles = new Vector3(0f, 0f, lookingAngle);

        Shoot();
    }
    public void Shoot()
    {
        canShoot = false;
        Debug.Log("Shoot"); ;
        shootMuzzle.Play();
        //GameObject bullet = pooler.SpawnFromPool("Bullet01", shootPoint.transform.position, shootPoint.transform.rotation);
        //Bullet bulletCs = bullet.GetComponent<Bullet>();

        Vector3 direction = (enemy.transform.position - gameObject.transform.position).normalized;

        /*LeanTween.move(bullet, shootPoint.transform.position + (direction * shootRange), 1f / bulletSpeed).setOnComplete(() =>
        {

        });*/

        LeanTween.delayedCall((1f / attackSpeed), () =>
        {
            canShoot = true;
        });
    }
}
