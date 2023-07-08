using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseItemVolume : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColor;

    private PlayerControlSystem playerControls;
    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;

    private void Awake()
    {
        playerControls = new PlayerControlSystem();
        playerControls.Player.UseItem.performed += _ => CheckUseItem();
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Cursor Is Entering Entity.");
        if (InventoryController.main.IsInventoryActive())
        {
            isSelected = true;
            spriteRenderer.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Cursor Is Exiting Entity.");
        if (InventoryController.main.IsInventoryActive())
        {
            ResetVolume();
        }
    }

    public void ResetVolume()
    {
        isSelected = false;
        spriteRenderer.color = normalColor;
    }

    private void CheckUseItem()
    {
        //Debug.Log("Checking For Item Use...");

        //If the used volume is hovered over, the right piece is selected, use the item
        if (InventoryController.main.activeInventoryID >= 0 && isSelected)
        {
            Debug.Log("Current ID: " + InventoryController.main.activeInventoryID);
            Debug.Log("Item Successfully Used!");
            InventoryController.main.hasSuccessfulInteraction = true;
        }
    }
}
