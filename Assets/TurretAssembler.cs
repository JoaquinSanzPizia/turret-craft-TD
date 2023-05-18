using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAssembler : MonoBehaviour
{
    [SerializeField] TurretController turretController;

    [SerializeField] Color[] tierColors;
    public enum Element { fire, ice, poison, lightning};
    public Element element;

    public enum LChasisType { standart, assault, heavy}
    [Header("[LOWER CHASIS]")]
    public LChasisType lChasisType;

    public enum LChasisTier { common, uncommon, rare, epic, legendary }
    public LChasisTier lChasisTier;

    public enum UChasisType { compact, Vshape, assault }
    [Header("[UPPER CHASIS]")]
    public UChasisType uChasisType;

    public enum UChasisTier { common, uncommon, rare, epic, legendary }
    public UChasisTier uChasisTier;

    public enum CannonType { gun, wave, sprinkle }
    [Header("[CANNON]")]
    public CannonType cannonType;

    public enum CannonTier { common, uncommon, rare, epic, legendary }
    public CannonTier cannonTier;

    [Header("[PARTS GO]")]
    [SerializeField] GameObject[] lChasis;
    [SerializeField] GameObject[] uChasis;
    [SerializeField] GameObject[] cannons;

    [Header("[PARTS SO]")]
    [SerializeField] LowerChasisSO[] lChasisSO;
    [SerializeField] UpperChasisSO[] uChasisSO;
    [SerializeField] CannonSO[] cannonsSO;


    void Start()
    {
        AssembleTurret();
    }

    void AssembleTurret()
    {
        GetLChasis();
        GetUChasis();
        GetCannon();
    }

    void GetLChasis()
    {
        foreach(GameObject chasis in lChasis)
        {
            chasis.SetActive(false);
        }

        lChasis[(int)lChasisType].SetActive(true);
        LeanTween.color(lChasis[(int)lChasisType].transform.GetChild(0).gameObject, tierColors[((int)lChasisTier)], 0f);

        turretController.range = lChasisSO[((int)lChasisType)].rangeMultipliers[((int)lChasisTier)];
        turretController.fireRate = lChasisSO[((int)lChasisType)].fireRateMultipliers[((int)lChasisTier)];

        turretController.UpdateRangeSphere();
    }

    void GetUChasis()
    {
        foreach (GameObject chasis in uChasis)
        {
            chasis.SetActive(false);
        }

        uChasis[(int)uChasisType].SetActive(true);
        LeanTween.color(uChasis[(int)uChasisType].transform.GetChild(0).gameObject, tierColors[((int)uChasisTier)], 0f);

        turretController.elementMultiplier = uChasisSO[((int)uChasisType)].elementBoosts[((int)uChasisTier)];
        turretController.extraEffect = uChasisSO[((int)uChasisType)].extraEffect;
    }

    void GetCannon()
    {
        foreach (GameObject cannon in cannons)
        {
            cannon.SetActive(false);
        }

        cannons[(int)cannonType].SetActive(true);
        LeanTween.color(cannons[(int)cannonType].transform.GetChild(0).gameObject, tierColors[((int)cannonTier)], 0f);

        turretController.damage = cannonsSO[((int)cannonType)].damage[((int)cannonTier)];
        turretController.bulletType = cannonsSO[((int)cannonType)].bulletType;
    }
}
