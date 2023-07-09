using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody2D))]
public class AppleMinigameControls : MonoBehaviour
{

    [SerializeField]
    public float speed;
    private PlayerControlSystem playerCons;
    private Vector2 moveInput;
    private Rigidbody2D rbKaiju;
    private bool playing;
    private AppleMinigame mgCode;

    private void Awake()
    {
        mgCode = GetComponentInParent<AppleMinigame>();
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
			if (moveInput.x < 0f)
			{
				rbKaiju.gameObject.GetComponent<SpriteRenderer>().flipX = true;
			}
			if (moveInput.x > 0f)
			{
				rbKaiju.gameObject.GetComponent<SpriteRenderer>().flipX = false;
			}
            rbKaiju.transform.Translate(moveInput * speed * 0.1f);
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
