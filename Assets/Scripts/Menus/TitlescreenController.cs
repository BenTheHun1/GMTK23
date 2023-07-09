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

    private bool transitionActive = false;

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
            SwitchMenu((int)MenuState.MAIN);
    }

    public void SwitchMenu(int newMenuState)
    {
        if (!transitionActive && !GameData.inGame)
        {
            exitMenuState = currentMenuState;
            currentMenuState = (MenuState)newMenuState;
            AnimateMenuState(exitMenuState, false);
            AnimateMenuState(currentMenuState, true);
        }
    }

    private void AnimateMenuState(MenuState menuState, bool isEntering)
    {
        transitionActive = true;
        switch (menuState)
        {
            case MenuState.START:
                if (!isEntering)
                {
                    LeanTween.moveY(menuStates[(int)MenuState.START], startScreenEndPos, menuAnimationDuration).setEase(slideMenuEaseType).setOnComplete(() => transitionActive = false);
                }
                break;
            case MenuState.MAIN:
                if (isEntering)
                {
                    LeanTween.moveY(menuStates[(int)MenuState.MAIN], mainMenuEndPos, menuAnimationDuration).setEase(slideMenuEaseType).setOnComplete(() => transitionActive = false);
                }
                break;
        }
    }

    public void PlayGame()
    {
        if (!transitionActive && !GameData.inGame)
        {
            IntroAnimation();
        }
    }

    private void IntroAnimation()
    {
        transitionActive = true;
        LeanTween.alphaCanvas(whiteScreen, 1f, fadeInDuration).setEase(fadeInEaseType).setOnComplete(InitializeGameSequence);
    }

    private void InitializeGameSequence()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        LeanTween.alphaCanvas(whiteScreen, 0f, fadeOutDuration).setEase(fadeOutEaseType).setOnComplete(() => GameManager.instance.Init());
        transitionActive = false;
    }

    public void QuitGame()
    {
        if (!transitionActive && !GameData.inGame)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
