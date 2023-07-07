using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public int ID;
    public string name;
    public string description;
    public int quantity;
    public Sprite itemImage;
    public Color imageColor;

    //This is a custom Item class created to store groups of data for one Item object
    public Item(int itemID, string itemName, string itemDescription, int itemQuantity, SpriteRenderer itemImg)
    {
        ID = itemID;
        name = itemName;
        description = itemDescription;
        quantity = itemQuantity;
        itemImage = itemImg.sprite;
        imageColor = itemImg.color;
    }
}

