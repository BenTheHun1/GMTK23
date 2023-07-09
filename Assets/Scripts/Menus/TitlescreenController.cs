using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlescreenController : MonoBehaviour
{
    private enum MenuState { START, MAIN, OPTIONS, HOWTOPLAY };
    [SerializeField] private RectTransform[] menuStates;

    [SerializeField] private float startScreenStartPos;
    [SerializeField] private float startScreenEndPos;

    [SerializeField] private float mainMenuStartPos;
    [SerializeField] private float mainMenuEndPos;

    [SerializeField] private float menuAnimationDuration;
    [SerializeField] private LeanTweenType slideMenuEaseType;

    [SerializeField] private CanvasGroup whiteScreen;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float delayDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private LeanTweenType fadeInEaseType;
    [SerializeField] private LeanTweenType fadeOutEaseType;

    private MenuState currentMenuState;
    private MenuState exitMenuState;

    private PlayerControlSystem playerControlSystem;

    private void Awake()
    {
        playerControlSystem = new PlayerControlSystem();
        playerControlSystem.Player.AnyButton.performed += _ => StartMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        exitMenuState = MenuState.START;
        currentMenuState = MenuState.START;
    }

    private void OnEnable()
    {
        playerControlSystem.Enable();
    }

    private void OnDisable()
    {
        playerControlSystem.Disable();
    }

    private void StartMenu()
    {
        if(currentMenuState == MenuState.START)
        {
            SwitchMenu((int)MenuState.MAIN);
            FindObjectOfType<AudioManager>().PlayOneShot("StartScreen", GameData.GetSFXVolume());
        }
    }

    public void SwitchMenu(int newMenuState)
    {
        if (!GameData.transitionActive && !GameData.inGame)
        {
            exitMenuState = currentMenuState;
            currentMenuState = (MenuState)newMenuState;
            AnimateMenuState(exitMenuState, false);
            AnimateMenuState(currentMenuState, true);
        }
    }

    private void AnimateMenuState(MenuState menuState, bool isEntering)
    {
        GameData.transitionActive = true;
        switch (menuState)
        {
            case MenuState.START:
                if (!isEntering)
                {
                    LeanTween.moveY(menuStates[(int)MenuState.START], startScreenEndPos, menuAnimationDuration).setEase(slideMenuEaseType).setOnComplete(() => GameData.transitionActive = false);
                }
                break;
            case MenuState.MAIN:
                if (isEntering)
                {
                    LeanTween.moveY(menuStates[(int)MenuState.MAIN], mainMenuEndPos, menuAnimationDuration).setEase(slideMenuEaseType).setOnComplete(() => GameData.transitionActive = false);
                }
                break;
        }
    }

    public void PlayGame()
    {
        if (!GameData.transitionActive && !GameData.inGame)
        {
            IntroAnimation();
        }
    }

    private void IntroAnimation()
    {
        GameData.transitionActive = true;
        GameManager.instance.StopMusic();
        FindObjectOfType<AudioManager>().PlayOneShot("StartGameTone", GameData.GetSFXVolume());
        LeanTween.alphaCanvas(whiteScreen, 1f, fadeInDuration).setEase(fadeInEaseType).setOnComplete(InitializeGameSequence);
    }

    private void InitializeGameSequence()
    {
        GameManager.instance.ChangeMusic("GameMusic");
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        LeanTween.alphaCanvas(whiteScreen, 0f, fadeOutDuration).setEase(fadeOutEaseType).setOnComplete(() => GameData.transitionActive = false);
    }

    public void QuitGame()
    {
        if (!GameData.transitionActive && !GameData.inGame)
            GameManager.instance.QuitApplication();
    }
}
