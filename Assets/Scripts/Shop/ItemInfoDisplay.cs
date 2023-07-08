using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoDisplay : MonoBehaviour
{
    private TextMeshProUGUI itemInfoText;

    private void Start()
    {
        itemInfoText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateItemInformation(string info)
    {
        itemInfoText.text = info;
    }
}
