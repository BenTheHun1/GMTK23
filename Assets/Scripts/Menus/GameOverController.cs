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

        GameManager.instance.StopMusic();

		deathMessageText.text = GameData.kaijuName + " Has Passed Away.";

        LeanTween.alphaCanvas(canvasGroup, 1f, gameOverAlphaDuration).setEase(gameOverAlphaEaseType);
    }

    public void ShowGameEndScreen()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        gameEndTransform.gameObject.SetActive(true);
		BG.gameObject.SetActive(true);

		gameEndMessageText.text = "The time has passed.";

        LeanTween.alphaCanvas(canvasGroup, 1f, gameOverAlphaDuration).setEase(gameOverAlphaEaseType);
    }

    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public void Restart()
    {
        GameManager.instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitApplication();
    }
}
