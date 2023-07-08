using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatMinigameControls : MonoBehaviour
{

    private PlayerControlSystem playerCons;
    private bool playing;
    private RatMinigame mgCode;

    public GameObject leftbasket,
        rightbasket;

    private Animator leftbasketAnim,
        rightbasketAnim;

    // Start is called before the first frame update
    void Awake()
    {
        leftbasketAnim = leftbasket.GetComponent<Animator>();
        rightbasketAnim = rightbasket.GetComponent<Animator>();
        mgCode = GetComponentInParent<RatMinigame>();
        playerCons = new PlayerControlSystem();
    }

    private void FixedUpdate()
    {
        if (mgCode.spawn)
        {
            EnableBaskets();
        }
        else
        {
            DisableBaskets();
        }
        if (playing)
        {
            playerCons.Baskets.RightBasket.performed += RightBasketTriggered;
            playerCons.Baskets.LeftBasket.performed += LeftBasketTriggered;
        }
    }

    public void EnableBaskets()
    {
        playing = true;
    }

    public void DisableBaskets()
    {
        playing = false;
    }


    private void LeftBasketTriggered(InputAction.CallbackContext obj)
    {
        leftbasketAnim.SetTrigger("trigger");
    }

    private void RightBasketTriggered(InputAction.CallbackContext obj)
    {
        rightbasketAnim.SetTrigger("trigger");
    }


    private void OnEnable()
    {
        playerCons.Baskets.Enable();
    }

    private void OnDisable()
    {
        playerCons.Baskets.Disable();
    }
}
