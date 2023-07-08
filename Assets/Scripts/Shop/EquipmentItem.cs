using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField, Tooltip("The equipment image UI.")] private Image itemImage;
    [SerializeField, Tooltip("The text for the name of the equipment.")] private TextMeshProUGUI itemNameText;
    [SerializeField, Tooltip("The text that displays whether the equipment is on or not.")] private TextMeshProUGUI equipText;

    private Hat equipmentInfo;

    public void AddEquipmentInformation(Hat newEquipmentInfo)
    {
        equipmentInfo = newEquipmentInfo;
        DisplayEquipmentInfo();
    }

    /// <summary>
    /// Displays the equipment information in the UI.
    /// </summary>
    private void DisplayEquipmentInfo()
    {
        if (equipmentInfo.sprite != null)
            itemImage.sprite = equipmentInfo.sprite;
        else
            itemImage.color = new Color(0, 0, 0, 0);

        itemNameText.text = equipmentInfo.name.ToString();
        UpdateEquipText();
    }

    public void UpdateEquipText()
    {
        if (equipmentInfo == GameData.currentEquipment)
        {
            equipText.text = "Equipped";
            GetComponent<Button>().interactable = false;
        }
        else
        {
            equipText.text = "Equip";
            GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// Equips the equipment on the Kaiju.
    /// </summary>
    public void Equip()
    {
        //Load the hat onto the Kaiju
        FindObjectOfType<KaijuStats>().LoadHat(equipmentInfo);

        //Update the list so that any previously equipped equipments are unequipped
        foreach (var equip in FindObjectsOfType<EquipmentItem>())
            equip.UpdateEquipText();
    }
}
