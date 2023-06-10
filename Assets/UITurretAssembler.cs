using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurretAssembler : MonoBehaviour
{
    [SerializeField] AssembleWindow[] assembleWindows;

    public GameObject mainAssembleWindow;
    public GameObject currentTurret;

    public void AssembleTurretUI()
    {
        currentTurret.SetActive(true);
        currentTurret.GetComponentInChildren<TurretAssembler>().AssembleTurretWithIDs(assembleWindows[0].partID, assembleWindows[0].tierID,
                                                                                      assembleWindows[1].partID, assembleWindows[1].tierID,
                                                                                      assembleWindows[2].partID, assembleWindows[2].tierID,
                                                                                      assembleWindows[3].partID, assembleWindows[3].tierID);
        LeanTween.scale(mainAssembleWindow, Vector3.zero, 0.2f);
    }
}
