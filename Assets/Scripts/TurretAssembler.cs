using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAssembler : MonoBehaviour
{
    [SerializeField] TurretController turretController;

    [SerializeField] Color[] tierColors;
    [SerializeField] Color[] elementColors;
    public enum Element { fire, ice, poison, lightning, steel};
    public Element element;

    public enum LChasisType { standart, assault, heavy}
    [Header("[LOWER CHASIS]")]
    [Space]
    public LChasisType lChasisType;

    public enum LChasisTier { common, uncommon, rare, epic, legendary }
    public LChasisTier lChasisTier;

    public enum UChasisType { compact, Vshape, assault }
    [Header("[UPPER CHASIS]")]
    [Space]
    public UChasisType uChasisType;

    public enum UChasisTier { common, uncommon, rare, epic, legendary }
    public UChasisTier uChasisTier;

    public enum CannonType { gun, wave, laser }
    [Header("[CANNON]")]
    [Space]
    public CannonType cannonType;

    public enum CannonTier { common, uncommon, rare, epic, legendary }
    public CannonTier cannonTier;

    [Header("[PARTS GO]")]
    [Space]
    [SerializeField] GameObject[] lChasis;
    [SerializeField] GameObject[] uChasis;
    [SerializeField] GameObject[] cannons;

    [Header("[PARTS SO]")]
    [Space]
    [SerializeField] LowerChasisSO[] lChasisSO;
    [SerializeField] UpperChasisSO[] uChasisSO;
    [SerializeField] CannonSO[] cannonsSO;


    void Awake()
    {
        AssembleTurret();
    }

    void AssembleTurret()
    {
        GetVisuals();
        GetStats();
    }

    void GetVisuals()
    {
        turretController.bulletColor = elementColors[((int)element)];

        foreach (GameObject chasis in lChasis)
        {
            chasis.SetActive(false);
        }

        lChasis[(int)lChasisType].SetActive(true);
        LeanTween.color(lChasis[(int)lChasisType].transform.GetChild(0).gameObject, tierColors[((int)lChasisTier)], 0f);

        foreach (GameObject chasis in uChasis)
        {
            chasis.SetActive(false);
        }

        uChasis[(int)uChasisType].SetActive(true);
        LeanTween.color(uChasis[(int)uChasisType].transform.GetChild(0).gameObject, tierColors[((int)uChasisTier)], 0f);
        LeanTween.color(uChasis[(int)uChasisType].transform.GetChild(1).gameObject, elementColors[((int)element)], 0f);

        foreach (GameObject cannon in cannons)
        {
            cannon.SetActive(false);
        }

        cannons[(int)cannonType].SetActive(true);
        LeanTween.color(cannons[(int)cannonType].transform.GetChild(0).gameObject, tierColors[((int)cannonTier)], 0f);

        turretController.shootPoint = cannons[(int)cannonType].transform.GetChild(1).gameObject;
    }

    void GetStats()
    {
        turretController.elementMultiplier = uChasisSO[((int)uChasisType)].elementBoosts[((int)uChasisTier)];
        turretController.extraEffect = uChasisSO[((int)uChasisType)].extraEffect;

        turretController.range = lChasisSO[((int)lChasisType)].rangeMultipliers[((int)lChasisTier)] *
                                 cannonsSO[((int)cannonType)].rangeMultiplier[((int)cannonTier)];
        turretController.fireRate = lChasisSO[((int)lChasisType)].fireRateMultipliers[((int)lChasisTier)] *
                                    cannonsSO[((int)cannonType)].fireRateMultiplier[((int)cannonTier)];

        turretController.damage = cannonsSO[((int)cannonType)].damage[((int)cannonTier)];
        turretController.bulletType = cannonsSO[((int)cannonType)].bulletType;
        turretController.bulletSpeed = cannonsSO[((int)cannonType)].bulletSpeedMultiplier[((int)cannonTier)];

        turretController.element = (TurretController.Element)(Element)((int)element);
        turretController.elementTier = ((int)uChasisTier);

        turretController.UpdateRangeSphere();
    }
}
