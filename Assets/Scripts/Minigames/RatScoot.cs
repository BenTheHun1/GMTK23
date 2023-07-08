using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScoot : MonoBehaviour
{

    [SerializeField] private float speed;
    private float xLimit;
    [SerializeField] private int direction;
    private bool yChange,
        directionX;

    // Start is called before the first frame update
    void Start()
    {
        directionX = true;
        yChange = false;
        xLimit = 12.0f;
        speed = Random.Range(5.0f, 8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (directionX)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
            if (transform.position.y < -4.25f)
            {
                ChangeDirection();
            }
        }
        if (yChange)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed * direction);
        }


        if (transform.position.x < -xLimit || transform.position.x > xLimit)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDirection()
    {
        directionX = false;
        yChange = true;
        direction = Random.Range(-1, 2);
        if(direction == 0)
        {
            direction = -1;
        }
    }

}
