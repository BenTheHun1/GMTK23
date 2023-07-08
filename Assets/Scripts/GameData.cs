using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public Item itemData;
    public int quantity;

    public ItemData (Item itemData, int quantity = 1)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
}

public static class GameData
{
    internal static int currency = 1000;
    internal static List<ItemData> inventory = new List<ItemData>();

    public static void AddToInventory(Item itemData)
    {
        bool duplicateItem = false;
        for(int i = 0; i < inventory.Count; i++)
        {
            //If the player already has the item in their inventory, add to their quantity
            if (inventory[i].itemData.ID == itemData.ID)
            {
                inventory[i].quantity++;
                duplicateItem = true;
                break;
            }
        }

        //If the current object is not a duplicate of an existing one, add a new item to the inventory
        if (!duplicateItem)
            inventory.Add(new ItemData(itemData));

    }

    public static void RemoveFromInventory(int itemID)
    {
        bool deleteItem = true;
        int counter = 0;

        for (int i = 0; i < inventory.Count; i++)
        {
            //If the item trying to be removed has been found, stop looking for it with the break statement
            if (inventory[i].itemData.ID == itemID)
            {
                //If there's more than one of the item, just reduce the quantity
                if (inventory[i].quantity > 1)
                {
                    inventory[i].quantity--;
                    deleteItem = false;
                }
                //Forcefully exits the foreach loop
                break;
            }
            counter++;
        }

        //If the item needs to be deleted from the inventory, remove it from the inventory list
        if (deleteItem)
        {
            inventory.RemoveAt(counter);
        }
    }
}
