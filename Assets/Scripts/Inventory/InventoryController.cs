using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private RectTransform inventoryUI;
    [SerializeField] private Image[] imageGrid;
    [SerializeField] private SpriteRenderer testSprite;

    [SerializeField] private float inventoryHiddenPos;
    [SerializeField] private float inventoryDisplayedPos;
    [SerializeField] private float inventoryAnimationDuration;
    [SerializeField] private LeanTweenType inventoryEaseType;

    private PlayerControlSystem playerControls;
    private bool isInventoryActive;

    internal bool isDragging, isHidden, hasSuccessfulInteraction;
    internal int activeInventoryID, activeSiblingIndex;

    public static InventoryController main;

    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.Inventory.performed += _ => ToggleInventory();
        isDragging = false;
        isHidden = false;
        hasSuccessfulInteraction = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isInventoryActive = false;
        activeInventoryID = -1;
        activeSiblingIndex = -1;

        GameData.AddToInventory(new Item(-1, "TestItem", "This is a test item.", 1, testSprite));
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void ToggleInventory()
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

        Debug.Log("Inventory Active: " + isInventoryActive);
        ToggleAnimation();

        //Display all of the items in the inventory
        if (isInventoryActive)
            DisplayInventory();

        else
            activeInventoryID = -1;
    }

    private void ToggleAnimation()
    {
        LeanTween.moveX(inventoryUI, isInventoryActive? inventoryDisplayedPos : inventoryHiddenPos, inventoryAnimationDuration).setEase(inventoryEaseType);
    }

    private void DisplayInventory()
    {
        int counter = 0;
        foreach (var i in GameData.inventory)
        {

            imageGrid[counter].sprite = i.itemImage;
            imageGrid[counter].color = i.imageColor;
            imageGrid[counter].gameObject.GetComponentInParent<GridPieceEvent>().SetInventoryID(i.ID);
            counter++;
        }
    }

    private void ClearInventoryDisplay()
    {
        foreach (var i in imageGrid)
        {
            i.sprite = null;
            i.color = new Color(0, 0, 0, 0);
        }
    }

    public void CheckSuccessfulInteraction()
    {
        if (hasSuccessfulInteraction)
        {
            //Get rid of item from inventory
            GameData.RemoveFromInventory(activeInventoryID);
            //Clear the inventory display so that it can be updated
            ClearInventoryDisplay();
            //Make sure the inventory is inactive
            isInventoryActive = false;
            hasSuccessfulInteraction = false;
        }

        ToggleAnimation();

        //Create the inventory display again with the updated information
        if (isInventoryActive)
        {
            DisplayInventory();
        }
    }
}
