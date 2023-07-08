using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private enum ShopType { MAIN, FOOD, EQUIPMENT, SELL }

    private PlayerControlSystem playerControls;

    [SerializeField, Tooltip("The RectTransform of the main shop.")] private RectTransform mainShopTransform;

    [SerializeField, Tooltip("The position of the shop UI y-axis when hidden.")] private float shopStartPos;
    [SerializeField, Tooltip("The position of the shop UI y-axis when shown.")] private float shopEndPos;
    [SerializeField, Tooltip("The time it takes in seconds to enter the shop menu.")] private float shopEnterAnimationDuration;
    [SerializeField, Tooltip("The time it takes in seconds to exit the shop menu.")] private float shopExitAnimationDuration;
    [SerializeField, Tooltip("The ease types for the enter / exit animations.")] private LeanTweenType shopEnterEaseType, shopExitEaseType;

    [SerializeField, Tooltip("The time it takes in seconds for the main shop to fade in / out.")] private float shopFadeAnimationDuration;
    [SerializeField, Tooltip("The ease type for the main shop enter / exit animations.")] private LeanTweenType shopFadeEaseType;

    [SerializeField, Tooltip("The time it takes in seconds for the food shop to scale into / out of view.")] private float subShopAnimationDuration;
    [SerializeField, Tooltip("The ease types for the food shop enter / exit animations.")] private LeanTweenType subShopAnimationEaseType;
    [SerializeField, Tooltip("The time it takes in seconds for the food shop labels to fade in / out.")] private float fadeInLabelDuration;
    [SerializeField, Tooltip("The ease type for the fade in / out label animations.")] private LeanTweenType fadeInLabelEaseType;

    [SerializeField, Tooltip("The list of available shop menus.")] private GameObject[] shopMenus;

    [SerializeField, Tooltip("The list of possible items in the food store")] private Item[] foodItems;
    [SerializeField, Tooltip("The list of possible items in the equipment store")] private Hat[] equipmentItems;

    public static ShopController main;

    private Item[] foodItemInstances;
    private Hat[] equipmentItemInstances;

    private bool shopActive = false;
    private bool transitionActive = false;

    private int currentMenuID = -1;
    private int exitMenuID = -1;

    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.ToggleShop.performed += _ => ToggleShop();
        CreateInstances();
    }

    /// <summary>
    /// Creates instances of ScriptableObject arrays so that any changes are not saved.
    /// </summary>
    private void CreateInstances()
    {
        foodItemInstances = new Item[foodItems.Length];
        equipmentItemInstances = new Hat[equipmentItems.Length];

        for (int i = 0; i < foodItems.Length; i++)
            foodItemInstances[i] = Instantiate(foodItems[i]);

        for (int i = 0; i < equipmentItems.Length; i++)
            equipmentItemInstances[i] = Instantiate(equipmentItems[i]);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    /// <summary>
    /// Toggles the shop menu.
    /// </summary>
    public void ToggleShop()
    {
        if (!transitionActive && currentMenuID <= (int)ShopType.MAIN && !InventoryController.main.IsInventoryActive())
        {
            shopActive = !shopActive;
            transitionActive = true;

            currentMenuID = shopActive ? 0 : -1;

            //Move the shop menu on the y-axis when activating
            LeanTween.moveY(mainShopTransform, shopActive ? shopEndPos : shopStartPos, shopActive ? shopEnterAnimationDuration : shopExitAnimationDuration).setEase(shopActive ? shopEnterEaseType : shopExitEaseType)
                .setOnComplete(() => transitionActive = false);

            MainShopAnimation();
        }
    }

    /// <summary>
    /// Switches between menus.
    /// </summary>
    /// <param name="menuID">The menu ID to switch to. (0 = MAIN, 1 = FOOD, 2 = EQUIPMENT, 3 = SELL)</param>
    public void SwitchMenu(int menuID)
    {
        exitMenuID = currentMenuID;
        currentMenuID = menuID;

        switch ((ShopType)menuID)
        {
            case ShopType.MAIN:
                MainShopAnimation();
                break;
            case ShopType.FOOD:
                GetComponentInChildren<FoodShopController>().UpdateFoodList();
                SubShopAnimation((ShopType)menuID);
                break;
            case ShopType.EQUIPMENT:
                GetComponentInChildren<EquipmentShopController>().UpdateEquipmentList();
                SubShopAnimation((ShopType)menuID);
                break;
        }

        switch ((ShopType)exitMenuID)
        {
            case ShopType.MAIN:
                MainShopAnimation();
                break;
            case ShopType.FOOD:
                SubShopAnimation((ShopType)exitMenuID);
                break;
            case ShopType.EQUIPMENT:
                SubShopAnimation((ShopType)exitMenuID);
                break;
        }
    }

    /// <summary>
    /// Animates the main shop UI when entering / exiting the menu.
    /// </summary>
    private void MainShopAnimation()
    {
        transitionActive = true;
        float endingAlpha = currentMenuID == (int)ShopType.MAIN ? 1 : 0;

        float delay = 0;

        //Wait for other menus to finish their animations before fading back in
        switch ((ShopType)exitMenuID)
        {
            case ShopType.FOOD:
                delay = subShopAnimationDuration;
                break;
            case ShopType.EQUIPMENT:
                delay = subShopAnimationDuration;
                break;
        }

        //Fade in / out the main shop menu when entering / exiting
        shopMenus[(int)ShopType.MAIN].GetComponent<CanvasGroup>().alpha = endingAlpha == 0 ? 1 : 0;
        LeanTween.delayedCall(delay, () => LeanTween.alphaCanvas(shopMenus[(int)ShopType.MAIN].GetComponent<CanvasGroup>(), endingAlpha, shopFadeAnimationDuration).setEase(shopFadeEaseType).setOnComplete(() => transitionActive = false));
    }

    /// <summary>
    /// Animates the sub shops UI when entering / exiting the menu.
    /// </summary>
    private void SubShopAnimation(ShopType subShop)
    {
        transitionActive = true;
        float startingScale = currentMenuID == (int)subShop ? 0 : 1;
        float endingScale = currentMenuID == (int)subShop ? 1 : 0;

        shopMenus[(int)subShop].transform.localScale = new Vector3(1, startingScale, 1);

        if (currentMenuID == (int)subShop)
        {
            //When entering the food shop, scale the Y and then fade in the labels
            LeanTween.scaleY(shopMenus[(int)subShop], endingScale, subShopAnimationDuration).setEase(subShopAnimationEaseType).setOnComplete(() =>
            LeanTween.alphaCanvas(shopMenus[(int)subShop].GetComponentInChildren<CanvasGroup>(), endingScale, fadeInLabelDuration).setEase(fadeInLabelEaseType).setOnComplete(() => transitionActive = false));

            string shopMessage = "";

            switch (subShop)
            {
                case ShopType.FOOD:
                    shopMessage = "Purchase food for your Kaiju here.";
                    break;
                case ShopType.EQUIPMENT:
                    shopMessage = "Purchase equipment to enhance your Kaiju.";
                    break;
            }

            //Display a default message when transitioning into the sub shop
            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation(shopMessage);
        }
        else
        {
            //When exiting the sub shop, fade out the labels and then scale the Y
            LeanTween.alphaCanvas(shopMenus[(int)subShop].GetComponentInChildren<CanvasGroup>(), endingScale, fadeInLabelDuration).setEase(fadeInLabelEaseType).setOnComplete(() =>
            LeanTween.scaleY(shopMenus[(int)subShop], endingScale, subShopAnimationDuration).setEase(subShopAnimationEaseType).setOnComplete(() => transitionActive = false));
        }
    }

    /// <summary>
    /// Updates the currency and all currency displays.
    /// </summary>
    /// <param name="amount">The amount to update the currency by. Positive integers add currency and negative integers spend currency.</param>
    public void UpdateCurrency(int amount)
    {
        GameData.currency += amount;
        foreach(var currency in FindObjectsOfType<CurrencyDisplay>())
            currency.RefreshCurrency();
    }
    public bool IsShopActive() => shopActive;

    public Item[] GetMasterFoodList() => foodItemInstances;
    public Hat[] GetMasterEquipmentList() => equipmentItemInstances;
}
