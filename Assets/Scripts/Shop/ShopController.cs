using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private PlayerControlSystem playerControls;

    [SerializeField] private RectTransform mainShopTransform;

    [SerializeField] private float shopStartPos;
    [SerializeField] private float shopEndPos;
    [SerializeField] private float shopEnterAnimationDuration;
    [SerializeField] private float shopExitAnimationDuration;
    [SerializeField] private LeanTweenType shopEnterEaseType, shopExitEaseType;

    public static ShopController main;

    private bool shopActive = false;

    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.ToggleShop.performed += _ => ToggleShop();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void ToggleShop()
    {
        shopActive = !shopActive;
        LeanTween.moveY(mainShopTransform, shopActive? shopEndPos : shopStartPos, shopActive ? shopEnterAnimationDuration : shopExitAnimationDuration).setEase(shopActive ? shopEnterEaseType : shopExitEaseType);
    }

    public void UpdateCurrency(int amount)
    {
        GameData.currency += amount;
        foreach(var currency in FindObjectsOfType<CurrencyDisplay>())
            currency.RefreshCurrency();
    }
}
