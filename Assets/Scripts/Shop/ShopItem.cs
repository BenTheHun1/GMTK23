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

    private Item itemInfo;

    public void AddItemInformation(Item newItemInfo)
    {
        itemInfo = newItemInfo;
        DisplayItemInfo();
    }

    /// <summary>
    /// Displays the item information in the UI.
    /// </summary>
    private void DisplayItemInfo()
    {
        itemImage.sprite = itemInfo.itemImage;
        itemNameText.text = itemInfo.name;
        itemPriceText.text = itemInfo.price.ToString("n0") + " Yen";
    }

    /// <summary>
    /// Displays a description about the item when the item is selected in a menu.
    /// </summary>
    public void DisplayInfoOnSelect()
    {
        foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
            infoDisplay.UpdateItemInformation(itemInfo.description);
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
    /// Purchases the item that the player selects.
    /// </summary>
    public void PurchaseItem()
    {
        //If the player can afford to purchase the item, update the currency and add the item to their inventory
        if(itemInfo.price <= GameData.currency)
        {
            ShopController.main.UpdateCurrency(-itemInfo.price);
            GameData.AddToInventory(itemInfo);

            //Display that the item was purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation(itemInfo.name + " successfully purchased.");
        }
        else
        {
            //Display that the item could not be purchased
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("Not enough currency to buy this product.");
        }
    }
}
