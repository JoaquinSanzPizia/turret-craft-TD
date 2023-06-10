using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretPlatform : MonoBehaviour
{
    UITurretAssembler uIturretAssembler;
    public GameObject turret;
    public GameObject[] options;
    bool optionsOn;
    void Start()
    {
        HideOptions();
        uIturretAssembler = FindObjectOfType<UITurretAssembler>();
    }

    /*private void OnMouseDown()
    {
        if (optionsOn)
        {
            HideOptions();
            optionsOn = false;
        }
        else
        {
            ShowOptions();
            optionsOn = true;
        }
    }*/

    public void OptionsButton()
    {
        if (optionsOn)
        {
            HideOptions();
            optionsOn = false;
        }
        else
        {
            ShowOptions();
            optionsOn = true;
        }
    }

    void HideOptions()
    {
        foreach (GameObject option in options)
        {
            LeanTween.scale(option, Vector3.zero, 0.2f);
        }

        foreach (GameObject option in options)
        {
            LeanTween.moveLocalY(option, 0f, 0.2f);
        }
    }
    void ShowOptions()
    {
        foreach (GameObject option in options)
        {
            LeanTween.scale(option, Vector3.one, 0.2f);
        }
        LeanTween.moveLocalY(options[0], 0.26f, 0.2f);
        LeanTween.moveLocalY(options[1], -0.26f, 0.2f);
    }

    public void NewTurretButton()
    {
        uIturretAssembler.currentTurret = turret;
        LeanTween.scale(uIturretAssembler.mainAssembleWindow, Vector3.one, 0.2f);
    }

    public void SellTurret()
    {
        turret.SetActive(false);
    }
}
