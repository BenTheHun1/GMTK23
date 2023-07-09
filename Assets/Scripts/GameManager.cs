using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerControlSystem playerControlSystem;

    private void Awake()
    {
        instance = this;
        playerControlSystem = new PlayerControlSystem();
        playerControlSystem.Player.Quit.performed += _ => QuitApplication();
    }

    private void OnEnable()
    {
        playerControlSystem.Enable();
    }

    private void OnDisable()
    {
        playerControlSystem?.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("GameMusic", GameData.GetBGMVolume());
        FindObjectOfType<TriggeredTimer>().InitializeTimer();
    }

    public void Init()
    {
        GameData.inGame = true;
        FindObjectOfType<TriggeredTimer>().StartTimer();
    }

    public void GameOver()
    {
        GameData.inGame = false;
        FindObjectOfType<GameOverController>().ShowGameOverScreen();
    }

    public void GameEnd()
    {
        GameData.inGame = false;
        FindObjectOfType<GameOverController>().ShowGameEndScreen();
    }

    public void QuitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
