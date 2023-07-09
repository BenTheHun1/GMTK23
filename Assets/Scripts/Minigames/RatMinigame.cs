using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatMinigame : MonoBehaviour
{
    [SerializeField] private CurrencyAddedPanel currencyAddedPanel;
    [SerializeField] private int perfomanceBonus;
    [SerializeField, Range(0, 100)] private int perfomancePercentage;
    [SerializeField] private float gameLength = 30.0f;
    [SerializeField] private float timeRemaining;
    public bool spawn;
    private Vector2 spawnPoint;
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private GameObject ratHolder;
    public int score;
    public TMP_Text scoreText,
        timerText;

	public Item meat;

    private int totalRatsSpawned;

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
        timeRemaining = gameLength;
		score = 0;
        totalRatsSpawned = 0;
        GameData.miniGameActive = true;
        ClearRats();
        StartCoroutine(Spawn());
    }

    private void ClearRats()
    {
        foreach(Transform trans in ratHolder.transform)
            Destroy(trans.gameObject);
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

        float scorePercentage = (float)score / totalRatsSpawned * 100f;

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
            totalRatsSpawned++;
            yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
        }
    }

}
