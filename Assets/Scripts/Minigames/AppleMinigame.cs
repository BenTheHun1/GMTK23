using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AppleMinigame : MonoBehaviour
{
    [SerializeField] private CurrencyAddedPanel currencyAddedPanel;
    [SerializeField] private int perfomanceBonus;
    [SerializeField, Range(0, 100)] private int perfomancePercentage;
    [SerializeField] private float gameLength = 30.0f;
    [SerializeField] private float timeRemaining;
    public bool spawn;
    private Vector2 currentSpawn;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject appleHolder;
    public int score;
    public TMP_Text scoreText,
        timerText;
	public Item fruit;

    private int totalApplesSpawned;

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
        totalApplesSpawned = 0;
        score = 0;
        timeRemaining = gameLength;
        ClearApples();
        GameData.miniGameActive = true;
        StartCoroutine(Spawn());
    }

    private void ClearApples()
    {
        foreach (Transform trans in appleHolder.transform)
            Destroy(trans.gameObject);
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

        float scorePercentage = (float)score / totalApplesSpawned * 100f;

        if (scorePercentage >= perfomancePercentage)
        {
            ShopController.main.UpdateCurrency(perfomanceBonus);
            currencyAddedPanel.ShowCurrencyAlert(perfomanceBonus);
            Invoke("ReturnToMain", 4);
        }
        else
        {
            ReturnToMain();
        }
    }

    private void ReturnToMain()
    {
        GameData.miniGameActive = false;
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
            totalApplesSpawned++;

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }


    public Vector3 GetSpawnPoint()
    {
        currentSpawn = new Vector2((Random.Range(-9.0f, 9.0f)), 7.0f);
        return currentSpawn;
    }

}
