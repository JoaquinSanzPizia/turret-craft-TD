using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPoolableObject
{
    [SerializeField] SpriteRenderer model;
    [SerializeField] SphereCollider col;
    [SerializeField] ParticleSystem deathPS;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [SerializeField] GameObject healthBarFill, healthBar;

    private Transform originalPos;

    public bool alive;
    public void OnObjectSpawn()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GetHit(other.GetComponent<Bullet>().damage);
        }
    }

    void GetHit(int damage)
    {
        //LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0f);

        currentHealth -= damage;
        healthBarFill.GetComponent<Image>().fillAmount = (1f / maxHealth) * currentHealth;
        LeanTween.scale(gameObject, gameObject.transform.localScale * 1.1f, 0.1f).setLoopPingPong(1);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
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
    }
}