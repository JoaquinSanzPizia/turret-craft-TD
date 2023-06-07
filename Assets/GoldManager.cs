using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoldManager : MonoBehaviour
{
    [SerializeField] float currentGold;
    [SerializeField] TextMeshProUGUI goldText;

    private void Update()
    {
        goldText.text = $"{currentGold}";
    }
    public void GainGold(float goldToGain)
    {
        currentGold += goldToGain;
    }
}
