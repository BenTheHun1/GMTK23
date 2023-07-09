using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private RectTransform gameOverTransform;
    [SerializeField] private RectTransform gameEndTransform;

    [SerializeField] private TextMeshProUGUI deathMessageText;

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

        deathMessageText.text = GameData.kaijuName + " Has Passed Away.";

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
