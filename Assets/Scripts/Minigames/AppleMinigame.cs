using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AppleMinigame : MonoBehaviour
{

    [SerializeField] private float timeRemaining;
    public bool spawn;
    private Vector2 currentSpawn;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject appleHolder;
    public int score;
    public TMP_Text scoreText,
        timerText;
	public Item fruit;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.SetText((Mathf.FloorToInt(timeRemaining % 60)).ToString());
            }
            else
            {
                StopMinigame();
            }
        }
    }

    public void StartMinigame()
    {
        spawn = true;
		score = 0;
        timeRemaining = 30.0f;
        StartCoroutine(Spawn());
    }

    public void StopMinigame()
    {
        timerText.SetText("0");
        timeRemaining = 0;
        spawn = false;
		for (int i = 0; i < score; i++)
		{
			GameData.AddToInventory(fruit);
		}
		FindObjectOfType<KaijuStats>().StartFruitGame(false);
	}


    public void UpdateScore()
    {
        score += 1;
        scoreText.SetText(score.ToString());
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
        currentSpawn = new Vector2((Random.Range(-9.0f, 9.0f)), 7.0f);
        return currentSpawn;
    }

}
