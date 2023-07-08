using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShopController : MonoBehaviour
{
    [SerializeField, Tooltip("The shop item prefab.")] private ShopItem shopItemPrefab;
    [SerializeField, Tooltip("The shop item container.")] private Transform shopItemContainer;

    /// <summary>
    /// Updates the food list with any items that have not been displayed yet.
    /// </summary>
    public void UpdateFoodList()
    {
        Item[] foodList = ShopController.main.GetMasterFoodList();

        for(int i = 0; i < foodList.Length; i++)
        {
            if (!foodList[i].displayed)
            {
                ShopItem newShopItem = Instantiate(shopItemPrefab, shopItemContainer);
                newShopItem.AddItemInformation(foodList[i]);
                newShopItem.transform.SetSiblingIndex(i);
                foodList[i].displayed = true;
            }
        }
    }

}
