using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatMinigame : MonoBehaviour
{

    [SerializeField] private float timeRemaining;
    public bool spawn;
    private Vector2 spawnPoint;
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private GameObject ratHolder;
    public int score;
    public TMP_Text scoreText,
        timerText;

	public Item meat;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(0, 7);
        //StartMinigame();
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
        timeRemaining = 30.0f;
		score = 0;
		StartCoroutine(Spawn());
    }

    public void StopMinigame()
    {
        timerText.SetText("0");
        timeRemaining = 0;
        spawn = false;
		for (int i = 0; i < score; i++)
		{
			GameData.AddToInventory(meat);
		}
		FindObjectOfType<KaijuStats>().StartMeatGame(false);
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
            Instantiate(ratPrefab, spawnPoint, transform.rotation, ratHolder.transform);

            yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
        }
    }

}
