using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssembleWindow : MonoBehaviour
{
    [SerializeField] TurretUIPartSO[] partsSo;
    [SerializeField] string[] tiers;
    [SerializeField] Color[] tierColors;

    [SerializeField] Image partSprite;
    [SerializeField] Image partTopSprite;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI partName;
    [SerializeField] TextMeshProUGUI tierText;

    [SerializeField] public int partID;
    [SerializeField] public int tierID;

    private void Start()
    {
        SetPartUI();
    }
    public void SetPartUI()
    {
        partSprite.sprite = partsSo[partID].partSprite;
        partTopSprite.sprite = partsSo[partID].partTopSprite;
        partTopSprite.color = tierColors[tierID];

        tierText.text = tiers[tierID];
        partName.text = partsSo[partID].partName;
        description.text = partsSo[partID].description;
    }
    public void RightPartButton()
    {
        if (partID < partsSo.Length - 1)
        {
            partID++;
            SetPartUI();
        }
    }

    public void LeftPartButton()
    {
        if (partID > 0)
        {
            partID--;
            SetPartUI();
        }
    }

    public void RightTierButton()
    {
        if (tierID < tiers.Length - 1)
        {
            tierID++;
            SetPartUI();
        }
    }

    public void LeftTierButton()
    {
        if (tierID > 0)
        {
            tierID--;
            SetPartUI();
        }
    }
}
