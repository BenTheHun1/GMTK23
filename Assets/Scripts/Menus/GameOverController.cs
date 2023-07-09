using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private RectTransform gameOverTransform;
    [SerializeField] private RectTransform gameEndTransform;
	[SerializeField] private RectTransform BG;

	[SerializeField] private TextMeshProUGUI deathMessageText;
    [SerializeField] private TextMeshProUGUI gameEndMessageText;

    [SerializeField] private float gameOverAlphaDuration;
    [SerializeField] private LeanTweenType gameOverAlphaEaseType;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowGameOverScreen()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        gameOverTransform.gameObject.SetActive(true);
		BG.gameObject.SetActive(true);

		deathMessageText.text = GameData.kaijuName + " Has Passed Away.";

        LeanTween.alphaCanvas(canvasGroup, 1f, gameOverAlphaDuration).setEase(gameOverAlphaEaseType);
    }

    public void ShowGameEndScreen()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        gameEndTransform.gameObject.SetActive(true);
		BG.gameObject.SetActive(true);

		gameEndMessageText.text = "Humanity has come to attack " + GameData.kaijuName + ".\n";
		if (FindObjectOfType<KaijuStats>().kaijuLvl >= 2)
		{
			gameEndMessageText.text += "You raised your Kaiju right, and they defended themselves.";
		}
		else
		{
			gameEndMessageText.text += "Your Kaiju was destroyed by Humanity. Curses!";
		}

        LeanTween.alphaCanvas(canvasGroup, 1f, gameOverAlphaDuration).setEase(gameOverAlphaEaseType);
    }

    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitApplication();
    }
}
