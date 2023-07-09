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
			if (FindObjectOfType<KaijuStats>().kaiju.kaijuType == Kaiju.type.carnivore)
			{
				gameEndMessageText.text += "Your carnivorous Kaiju fought off the attacking soldiers. ";
			}
			else if (FindObjectOfType<KaijuStats>().kaiju.kaijuType == Kaiju.type.herbivore)
			{
				gameEndMessageText.text += "The attacking soldiers couldn't penetrate your herbivorous Kaiju's shell. ";
			}
			else if (FindObjectOfType<KaijuStats>().kaiju.kaijuType == Kaiju.type.omnivore)
			{
				gameEndMessageText.text += "Your omnivorous Kaiju flees into the ocean, away from the attacking soldiers. ";
			}

			gameEndMessageText.text += GameData.kaijuName + " lives to destroy Humanity another day.";


		}
		else
		{
			gameEndMessageText.text += "Your Kaiju was too weak, and was killed by the attacking soldiers.";
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
