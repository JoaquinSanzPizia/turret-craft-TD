using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour, IPoolableObject
{
    [SerializeField] ParticleSystem hitPS;
    [SerializeField] TrailRenderer trailRenderer;
    public Transform originalParent;
    public GameObject visual;
    public int tweenID;
    public float damage;

    public bool pierces;

    [SerializeField] SphereCollider col;

    private void OnEnable()
    {
        originalParent = transform.parent;
    }
    public void OnObjectSpawn()
    {
        visual.SetActive(true);
        trailRenderer.enabled = true;
        col.enabled = true;
        transform.SetParent(null);
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

        hitPS.gameObject.transform.SetParent(null);
        hitPS.Play();

        transform.SetParent(originalParent);
        LeanTween.moveLocal(gameObject, Vector3.zero, 0f);

        LeanTween.delayedCall(0.2f, () =>
        {
            hitPS.gameObject.transform.SetParent(gameObject.transform);
            LeanTween.moveLocal(hitPS.gameObject, Vector3.zero, 0f);
        });
    }
}
