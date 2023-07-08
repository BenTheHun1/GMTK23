using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentShopController : MonoBehaviour
{
    [SerializeField, Tooltip("The shop item prefab.")] private ShopItem shopItemPrefab;
    [SerializeField, Tooltip("The shop item container.")] private Transform shopItemContainer;

    [SerializeField, Tooltip("The equipment list prefab.")] private EquipmentItem equipmentItemPrefab;
    [SerializeField, Tooltip("The equipment list container.")] private Transform equipmentItemContainer;

    private void Awake()
    {
        //Hide the food store by default
        transform.localScale = new Vector3(1, 0, 1);
        GetComponentInChildren<CanvasGroup>().alpha = 0;
    }

    /// <summary>
    /// Updates the equipment list with any items that have not been displayed yet.
    /// </summary>
    public void UpdateEquipmentList()
    {
        Hat[] equipmentList = ShopController.main.GetMasterEquipmentList();

        for (int i = 0; i < equipmentList.Length; i++)
        {
            //If the current equipment list item is not displayed but is available, show it in the store
            if (!equipmentList[i].displayed && equipmentList[i].available)
            {
                ShopItem newShopItem = Instantiate(shopItemPrefab, shopItemContainer);
                newShopItem.AddEquipmentInformation(equipmentList[i]);
                newShopItem.transform.SetSiblingIndex(i);
                equipmentList[i].displayed = true;
            }
        }
    }

    public void InitializeEquipmentList()
    {
        List<Hat>equipmentList = GameData.equipment;

        for (int i = 0; i < equipmentList.Count; i++)
            AddEquipment(equipmentList[i]);
    }

    public void AddEquipment(Hat newEquipment)
    {
        EquipmentItem newShopItem = Instantiate(equipmentItemPrefab, equipmentItemContainer);
        newShopItem.AddEquipmentInformation(newEquipment);
    }
}
