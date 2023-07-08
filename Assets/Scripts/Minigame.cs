using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{

    [SerializeField] private float timeRemaining;
    private bool spawn;
    private Vector3 currentSpawn;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject appleHolder;

    // Start is called before the first frame update
    void Start()
    {
        StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
    }


    public void StartMinigame()
    {
        spawn = true;
        timeRemaining = 30.0f;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (spawn == true)
        {
            Instantiate(applePrefab, GetSpawnPoint(), transform.rotation, appleHolder.transform);

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }


    public Vector3 GetSpawnPoint()
    {
        currentSpawn = new Vector3((Random.Range(-9.0f, 9.0f)), 7.0f, 0.0f);
        return currentSpawn;
    }
}
