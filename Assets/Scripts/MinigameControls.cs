using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody2D))]
public class MinigameControls : MonoBehaviour
{

    [SerializeField]
    private float speed;
    private PlayerControlSystem playerCons;
    private Vector2 moveInput;
    private Rigidbody2D rbKaiju;
    private bool playing;
    private Minigame mgCode;

    private void Awake()
    {
        mgCode = GetComponentInParent<Minigame>();
        speed = 5.0f;
        playerCons = new PlayerControlSystem();
        rbKaiju = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (mgCode.spawn)
        {
            EnableKaiju();
        }
        else
        {
            DisableKaiju();
        }
        if (playing)
        {
            moveInput = playerCons.Player.Move.ReadValue<Vector2>();
            moveInput.y = 0f;
            rbKaiju.velocity = moveInput * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mgCode.UpdateScore();
        Destroy(collision.gameObject);
        Debug.Log("collided");
    }

    public void EnableKaiju()
    {
        playing = true;
    }

    public void DisableKaiju()
    {
        playing = false;
    }


    private void OnEnable()
    {
        playerCons.Player.Move.Enable();
    }

    private void OnDisable()
    {
        playerCons.Player.Move.Disable();
    }
}
