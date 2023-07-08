using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShopController : MonoBehaviour
{
    [SerializeField, Tooltip("The shop item prefab.")] private ShopItem shopItemPrefab;
    [SerializeField, Tooltip("The shop item container.")] private Transform shopItemContainer;

    private void Awake()
    {
        //Hide the food store by default
        transform.localScale = new Vector3(1, 0, 1);
        GetComponentInChildren<CanvasGroup>().alpha = 0;
    }

    /// <summary>
    /// Updates the food list with any items that have not been displayed yet.
    /// </summary>
    public void UpdateFoodList()
    {
        Item[] foodList = ShopController.main.GetMasterFoodList();

        for(int i = 0; i < foodList.Length; i++)
        {
            //If the current food list item is not displayed but is available, show it in the store
            if (!foodList[i].displayed && foodList[i].available)
            {
                ShopItem newShopItem = Instantiate(shopItemPrefab, shopItemContainer);
                newShopItem.AddItemInformation(foodList[i]);
                newShopItem.transform.SetSiblingIndex(i);
                foodList[i].displayed = true;
            }
        }
    }

}
