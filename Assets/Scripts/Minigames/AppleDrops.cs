using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleDrops : MonoBehaviour
{

    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if(transform.position.y < -8.0f)
        {
            Destroy(gameObject);
        }
    }
}
