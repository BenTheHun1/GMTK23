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

    [SerializeField] private float shopFadeAnimationDuration;
    [SerializeField] private LeanTweenType shopFadeEaseType;

    [SerializeField] private float foodShopAnimationDuration;
    [SerializeField] private LeanTweenType foodShopAnimationEaseType;
    [SerializeField] private float fadeInLabelDuration;
    [SerializeField] private LeanTweenType fadeInLabelEaseType;

    [SerializeField, Tooltip("The list of available shop menus.")] private GameObject[] shopMenus;

    [SerializeField, Tooltip("The list of possible items in the food store")] private Item[] foodItems;

    public static ShopController main;

    private Item[] foodItemInstances;
    private bool shopActive = false;
    private bool transitionActive = false;

    private int currentMenuID = (int)ShopType.MAIN;
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
        for (int i = 0; i < foodItems.Length; i++)
            foodItemInstances[i] = Instantiate(foodItems[i]);
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
    private void ToggleShop()
    {
        if (!transitionActive && currentMenuID == (int)ShopType.MAIN && !InventoryController.main.IsInventoryActive())
        {
            shopActive = !shopActive;
            transitionActive = true;

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
                FoodShopAnimation();
                break;
        }

        switch ((ShopType)exitMenuID)
        {
            case ShopType.MAIN:
                MainShopAnimation();
                break;
            case ShopType.FOOD:
                FoodShopAnimation();
                break;
        }
    }

    private void MainShopAnimation()
    {
        transitionActive = true;
        float endingAlpha = currentMenuID == (int)ShopType.MAIN ? 1 : 0;

        float delay = 0;

        switch ((ShopType)exitMenuID)
        {
            case ShopType.FOOD:
                delay = foodShopAnimationDuration;
                break;
        }

        shopMenus[(int)ShopType.MAIN].GetComponent<CanvasGroup>().alpha = endingAlpha == 0 ? 1 : 0;
        LeanTween.delayedCall(delay, () => LeanTween.alphaCanvas(shopMenus[(int)ShopType.MAIN].GetComponent<CanvasGroup>(), endingAlpha, shopFadeAnimationDuration).setEase(shopFadeEaseType).setOnComplete(() => transitionActive = false));
    }

    private void FoodShopAnimation()
    {
        transitionActive = true;
        float startingScale = currentMenuID == (int)ShopType.FOOD ? 0 : 1;
        float endingScale = currentMenuID == (int)ShopType.FOOD ? 1 : 0;

        shopMenus[(int)ShopType.FOOD].transform.localScale = new Vector3(1, startingScale, 1);

        if(currentMenuID == (int)ShopType.FOOD)
        {
            LeanTween.scaleY(shopMenus[(int)ShopType.FOOD], endingScale, foodShopAnimationDuration).setEase(foodShopAnimationEaseType).setOnComplete(() =>
            LeanTween.alphaCanvas(shopMenus[(int)ShopType.FOOD].GetComponentInChildren<CanvasGroup>(), endingScale, fadeInLabelDuration).setEase(fadeInLabelEaseType).setOnComplete(() => transitionActive = false));

            foreach (var infoDisplay in FindObjectsOfType<ItemInfoDisplay>())
                infoDisplay.UpdateItemInformation("Purchase food for your Kaiju here.");
        }
        else
        {
            LeanTween.alphaCanvas(shopMenus[(int)ShopType.FOOD].GetComponentInChildren<CanvasGroup>(), endingScale, fadeInLabelDuration).setEase(fadeInLabelEaseType).setOnComplete(() =>
            LeanTween.scaleY(shopMenus[(int)ShopType.FOOD], endingScale, foodShopAnimationDuration).setEase(foodShopAnimationEaseType).setOnComplete(() => transitionActive = false));
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
}
