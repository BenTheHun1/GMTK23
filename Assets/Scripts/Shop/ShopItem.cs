using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{

    [SerializeField, Tooltip("The item image UI.")] private Image itemImage;
    [SerializeField, Tooltip("The text for the name of the item.")] private TextMeshProUGUI itemNameText;
    [SerializeField, Tooltip("The text for the price of the item.")] private TextMeshProUGUI itemPriceText;
    [SerializeField, Tooltip("The label that showcases that the item is newly added.")] private GameObject newLabel;
    [SerializeField, Tooltip("The label that showcases that the item is sold out.")] private GameObject soldOutLabel;

    private Item itemInfo;
    private Hat equipmentInfo;

    public void AddItemInformation(Item newItemInfo)
    {
        itemInfo = newItemInfo;
        DisplayItemInfo();
    }

    public void AddEquipmentInformation(Hat newEquipmentInfo)
    {
        equipmentInfo = newEquipmentInfo;
        DisplayEquipmentInfo();
    }

    /// <summary>
    /// Displays the item information in the UI.
    /// </summary>
    private void DisplayItemInfo()
    {
        if (itemInfo.itemImage != null)
            itemImage.sprite = itemInfo.itemImage;
        else
            itemImage.color = new Color(0, 0, 0, 0);

        itemNameText.text = itemInfo.name.ToString();
        itemPriceText.text = itemInfo.price.ToString("n0") + " Yen";
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
        itemPriceText.text = equipmentInfo.price.ToString("n0") + " Yen";
    }

    /// <summary>
    /// Displays a description about the item when the item is selected in a menu.
    /// </summary>
    public void DisplayInfoOnSelect()
    {
        if(itemInfo != null)
        {
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation(itemInfo.description.ToString());
        }
        else
        {
            if (!equipmentInfo.soldOut)
            {
                foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                    infoDisplay.UpdateItemInformation(equipmentInfo.description.ToString());
            }
        }
    }

    /// <summary>
    /// Hides the "New!" label after deselecting the item for the first time.
    /// </summary>
    public void HideNewLabel()
    {
        if(newLabel.activeInHierarchy)
            newLabel.SetActive(false);
    }

    /// <summary>
    /// Shows the "Sold Out!" label after purchasing an item that cannot be purchased anymore.
    /// </summary>
    public void ShowSoldOutLabel()
    {
        if (!soldOutLabel.activeInHierarchy)
            soldOutLabel.SetActive(true);

        equipmentInfo.soldOut = true;
        GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Purchases either an item or equipment.
    /// </summary>
    public void Purchase()
    {
        if (itemInfo != null)
            PurchaseItem();
        else
            PurchaseEquipment();
    }

    /// <summary>
    /// Purchases the item that the player selects.
    /// </summary>
    private void PurchaseItem()
    {
        //If the player can afford to purchase the item, update the currency and add the item to their inventory
        if(itemInfo.price <= GameData.currency)
        {
            ShopController.main.UpdateCurrency(-itemInfo.price);
            GameData.AddToInventory(itemInfo);

            //Display that the item was purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("" + itemInfo.name.ToString() + " successfully purchased.");
        }
        else
        {
            //Display that the item could not be purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("Not enough currency to buy this product.");
        }
    }

    /// <summary>
    /// Purchases equipment that the player selects.
    /// </summary>
    private void PurchaseEquipment()
    {
        //If the player can afford to purchase the item, update the currency and add the item to their inventory
        if (equipmentInfo.price <= GameData.currency)
        {
            ShopController.main.UpdateCurrency(-equipmentInfo.price);
            GameData.AddToEquipment(equipmentInfo);

            //Display that the equipment was purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("" + equipmentInfo.name.ToString() + " successfully purchased.");

            //For equipment, set it to sold out
            ShowSoldOutLabel();

            FindObjectOfType<EquipmentShopController>().AddEquipment(equipmentInfo);
        }
        else
        {
            //Display that the item could not be purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("Not enough currency to buy this product.");
        }
    }
}
