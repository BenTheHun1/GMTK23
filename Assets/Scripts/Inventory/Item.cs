using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item: ScriptableObject
{
    public int ID;
    public new string name;
    public string description;
    public int price;
    public int quality;

    public Sprite itemImage;
    public Color imageColor = Color.white;
    public bool available = false;

    internal bool displayed = false;

    public void DisplayItem()
    {
        displayed = true;
    }
}