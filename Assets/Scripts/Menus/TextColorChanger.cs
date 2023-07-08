using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColorChanger : MonoBehaviour
{
    [SerializeField] private Color selectedTextColor;
    [SerializeField] private Color clickedTextColor;

    private Color defaultColor;

    private TextMeshProUGUI itemText;

    private void Start()
    {
        itemText = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = itemText.color;
    }

    public void ChangeToSelectColor()
    {
        itemText.color = selectedTextColor;
    }

    public void ChangeToClickedColor()
    {
        itemText.color = clickedTextColor;
    }

    public void ResetColor()
    {
        itemText.color = defaultColor;
    }
}
