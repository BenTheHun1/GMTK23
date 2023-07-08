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

        GameData.AddToInventory(new Item(0, "TestItem", "This is a test item.", 2, testSprite));
        GameData.AddToInventory(new Item(1, "TestItem", "This is a test item.", 2, testSprite));
        GameData.AddToInventory(new Item(2, "TestItem", "This is a test item.", 3, testSprite));
        GameData.AddToInventory(new Item(3, "TestItem", "This is a test item.", 4, testSprite));
        GameData.AddToInventory(new Item(4, "TestItem", "This is a test item.", 5, testSprite));
        GameData.AddToInventory(new Item(5, "TestItem", "This is a test item.", 6, testSprite));
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void ToggleInventory()
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
            imageGrid[counter].GetComponentInChildren<TextMeshProUGUI>().alpha = 1;
            imageGrid[counter].GetComponentInChildren<TextMeshProUGUI>().text = i.quantity.ToString();
            counter++;
        }
    }

    private void ClearInventoryDisplay()
    {
        foreach (var i in imageGrid)
        {
            i.sprite = null;
            i.color = new Color(0, 0, 0, 0);
            i.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
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
            hasSuccessfulInteraction = false;
        }

        //Create the inventory refresh with the updated information
        if (isInventoryActive)
            DisplayInventory();
    }

    public bool IsInventoryActive() => isInventoryActive;
}
