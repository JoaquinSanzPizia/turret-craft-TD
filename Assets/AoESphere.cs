using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoESphere : MonoBehaviour
{
    public SphereCollider col;
    public float sphereDamage;

    public void Activate(float damage, float radius)
    {
        sphereDamage = damage;
        col.gameObject.transform.localScale = new Vector3(radius, radius, radius);
        col.enabled = true;

        LeanTween.delayedCall(0.05f, () =>
        {
            col.enabled = false;
            col.gameObject.transform.localScale = Vector3.zero;
        });
    }
}
