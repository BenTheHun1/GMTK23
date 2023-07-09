using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI kaijuName;
    [SerializeField] private RectTransform howToPlayPanel;
    [SerializeField] private RectTransform namePanel;
    [SerializeField] private TextMeshProUGUI howToPlayText;
    [SerializeField] private TextMeshProUGUI pageNumberText;
    [SerializeField] private string[] howToPlayPassages;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private TMP_InputField nameInput;

    [SerializeField] private float nameInputExitDuration;
    [SerializeField] private LeanTweenType nameInputExitEaseType;

    private bool tutorialRead = false;
    private int currentTutorialPassage = 0;


    // Start is called before the first frame update
    void Start()
    {
        DisplayPassage();
    }

    public void ChangePage(int increment)
    {
        currentTutorialPassage += increment;
        currentTutorialPassage = Mathf.Clamp(currentTutorialPassage, 0, howToPlayPassages.Length - 1);

        DisplayPassage();
    }

    private void DisplayPassage()
    {
        howToPlayText.text = howToPlayPassages[currentTutorialPassage];
        pageNumberText.text = (currentTutorialPassage + 1).ToString() + " / " + howToPlayPassages.Length.ToString();

        previousButton.interactable = currentTutorialPassage > 0 ? true : false;
        nextButton.interactable = currentTutorialPassage < howToPlayPassages.Length - 1 ? true : false;

        if (currentTutorialPassage >= howToPlayPassages.Length - 1 && !tutorialRead)
        {
            continueButton.SetActive(true);
            tutorialRead = true;
        }
    }

    public void Dismiss()
    {
        howToPlayPanel.gameObject.SetActive(false);
        namePanel.gameObject.SetActive(true);
    }

    public void SetName()
    {
        string nameData = nameInput.text.ToUpper();

        if (nameData == "")
            nameData = "KAIJU";

        Debug.Log("Kaiju Name: " + nameData);

        GameData.kaijuName = nameData;

        if (kaijuName != null)
            kaijuName.text = GameData.kaijuName;

        LeanTween.scale(namePanel.gameObject, Vector3.zero, nameInputExitDuration).setEase(nameInputExitEaseType).setOnComplete(() => GameManager.instance.Init());
    }
}
