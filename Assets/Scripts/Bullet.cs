using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour, IPoolableObject
{
    public enum Element { fire, ice, poison, lightning, steel}
    public Element bulletElement;
    [SerializeField] TrailRenderer trailRenderer;
    public Transform originalParent;
    public GameObject visual;
    public int tweenID;
    public float damage;

    public bool pierces;

    public Color bulletColor;
    public ParticleSystem[] hitPsParts;
    public ParticleSystem[] shootMuzzle;

    [SerializeField] SphereCollider col;

    public int elementTier;

    private void OnEnable()
    {
        originalParent = transform.parent;
        SetParticleAndBulletColor();
    }
    public void OnObjectSpawn()
    {
        TurretController turret = originalParent.GetComponentInParent<TurretController>();
        visual.SetActive(true);
        trailRenderer.enabled = true;
        col.enabled = true;
        transform.SetParent(null);
        shootMuzzle[0].Play();

        bulletColor = turret.bulletColor;
        bulletElement = (Element)(int)turret.element;
        elementTier = turret.elementTier;

        SetParticleAndBulletColor();
    }

    void SetParticleAndBulletColor()
    {
        visual.GetComponent<SpriteRenderer>().color = bulletColor;
        trailRenderer.startColor = bulletColor;
        trailRenderer.endColor = bulletColor;

        foreach (ParticleSystem ps in shootMuzzle)
        {
            ParticleSystem.MainModule newPs = ps.main;
            newPs.startColor = bulletColor;
        }

        foreach (ParticleSystem ps in hitPsParts)
        {
            ParticleSystem.MainModule newPs = ps.main;
            newPs.startColor = bulletColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !pierces)
        {
            DisableBullet();
        }
    }

    public void DisableBullet()
    {
        trailRenderer.Clear();
        LeanTween.cancel(tweenID);

        trailRenderer.enabled = false;
        col.enabled = false;
        visual.SetActive(false);

        hitPsParts[0].gameObject.transform.SetParent(null);
        hitPsParts[0].Play();

        transform.SetParent(originalParent);
        LeanTween.moveLocal(gameObject, Vector3.zero, 0f);

        LeanTween.delayedCall(0.2f, () =>
        {
            hitPsParts[0].gameObject.transform.SetParent(gameObject.transform);
            LeanTween.moveLocal(hitPsParts[0].gameObject, Vector3.zero, 0f);
        });
    }
}
