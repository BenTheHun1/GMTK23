using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [SerializeField, Tooltip("The main container for the inventory UI.")] private RectTransform inventoryUI;
    [SerializeField, Tooltip("The images in the inventory grid.")] private Image[] imageGrid;

    [SerializeField, Tooltip("The x position for the inventory when hidden.")] private float inventoryHiddenPos;
    [SerializeField, Tooltip("The x position for the inventory when active.")] private float inventoryDisplayedPos;
    [SerializeField, Tooltip("The time in seconds for the inventory to move.")] private float inventoryAnimationDuration;
    [SerializeField, Tooltip("The ease type for the inventory animation.")] private LeanTweenType inventoryEaseType;

    private PlayerControlSystem playerControls;
    private bool isInventoryActive;

    internal bool isDragging, hasSuccessfulInteraction;
    internal int activeInventoryID;

    public static InventoryController main;

    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.Inventory.performed += _ => ToggleInventory();
        isDragging = false;
        hasSuccessfulInteraction = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isInventoryActive = false;
        activeInventoryID = -1;
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
    /// Toggles whether the inventory is active or hidden.
    /// </summary>
    public void ToggleInventory()
    {
        if (!GameData.transitionActive && GameData.inGame && !GameData.miniGameActive)
        {
            isInventoryActive = !isInventoryActive;

            if (!isInventoryActive)
            {
                //Hide any active tooltips if closing inventory
                foreach (var i in FindObjectsOfType<GridPieceEvent>())
                {
                    //Make sure nothing is hovered
                    i.ForceUnhover();
                }
            }

            //Debug.Log("Inventory Active: " + isInventoryActive);
            ToggleInventoryAnimation();

            //Display all of the items in the inventory
            if (isInventoryActive)
                DisplayInventory();

            else
                activeInventoryID = -1;
        }
    }

    /// <summary>
    /// Toggles the inventory UI animation.
    /// </summary>
    private void ToggleInventoryAnimation()
    {
        if (!GameData.transitionActive && !ShopController.main.IsShopActive())
        {
            GameData.transitionActive = true;
            LeanTween.moveX(inventoryUI, isInventoryActive ? inventoryDisplayedPos : inventoryHiddenPos, inventoryAnimationDuration).setEase(inventoryEaseType).setOnComplete(() => GameData.transitionActive = false);
        }
    }

    /// <summary>
    /// Takes the game data's inventory information and displays it in the game's UI.
    /// </summary>
    private void DisplayInventory()
    {
        int counter = 0;
        foreach (var i in GameData.inventory)
        {

            imageGrid[counter].sprite = i.itemData.itemImage;
            imageGrid[counter].color = i.itemData.imageColor;
            imageGrid[counter].gameObject.GetComponentInParent<GridPieceEvent>().SetInventoryID(i.itemData.ID);
            imageGrid[counter].GetComponentInChildren<TextMeshProUGUI>().alpha = 1;
            imageGrid[counter].GetComponentInChildren<TextMeshProUGUI>().text = i.quantity.ToString();
            counter++;
        }
    }

    /// <summary>
    /// Clears the inventory visual display.
    /// </summary>
    private void ClearInventoryDisplay()
    {
        foreach (var i in imageGrid)
        {
            i.sprite = null;
            i.color = new Color(0, 0, 0, 0);
            i.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
        }
    }

    /// <summary>
    /// Checks to see if the item dragged by the player has successfully interacted with something.
    /// </summary>
    public void CheckSuccessfulInteraction()
    {
        if (hasSuccessfulInteraction)
        {
            //Get rid of item from inventory
            GameData.RemoveFromInventory(activeInventoryID);
            //Clear the inventory display so that it can be updated
            ClearInventoryDisplay();
            hasSuccessfulInteraction = false;
        }

        //Create the inventory refresh with the updated information
        if (isInventoryActive)
            DisplayInventory();
    }

    public bool IsInventoryActive() => isInventoryActive;
}
