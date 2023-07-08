using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    internal static int currency = 1000;
    internal static List<Item> inventory = new List<Item>();

    public static void AddToInventory(Item itemData)
    {
        bool duplicateItem = false;
        foreach (var i in inventory)
        {
            //If the player already has the item in their inventory, add to their quantity
            if (i.ID == itemData.ID)
            {
                i.quantity++;
                duplicateItem = true;
                break;
            }
        }

        //If the current object is not a duplicate of an existing one, add a new item to the inventory
        if (!duplicateItem)
            inventory.Add(itemData);

    }

    public static void RemoveFromInventory(int itemID)
    {
        bool deleteItem = true;
        int counter = 0;
        foreach (var i in inventory)
        {
            //If the item trying to be removed has been found, stop looking for it with the break statement
            if (i.ID == itemID)
            {
                //If there's more than one of the item, just reduce the quantity
                if (i.quantity > 1)
                {
                    i.quantity--;
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
