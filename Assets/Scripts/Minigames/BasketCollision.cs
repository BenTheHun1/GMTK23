using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCollision : MonoBehaviour
{
    private bool playing;
    private RatMinigame mgCode;

    // Start is called before the first frame update
    void Start()
    {
        mgCode = GetComponentInParent<RatMinigame>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mgCode.UpdateScore();
        Destroy(collision.gameObject);
        Debug.Log("collided");
    }
}
