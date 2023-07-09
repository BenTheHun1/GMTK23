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

        ChangeMusic("TitlescreenMusic");
        FindObjectOfType<TriggeredTimer>().InitializeTimer();
    }

    public void ChangeMusic(string newMusicPlaying)
    {
        if (GameData.currentMusicPlaying != "" && FindObjectOfType<AudioManager>().IsPlaying(GameData.currentMusicPlaying))
            StopMusic();

        GameData.currentMusicPlaying = newMusicPlaying;
        FindObjectOfType<AudioManager>().Play(GameData.currentMusicPlaying, GameData.GetBGMVolume());
    }

    public void StopMusic()
    {
        if(GameData.currentMusicPlaying != "")
        {
            FindObjectOfType<AudioManager>().Stop(GameData.currentMusicPlaying);
            GameData.currentMusicPlaying = "";
        }
    }

    public void Init()
    {
        GameData.inGame = true;
        FindObjectOfType<TriggeredTimer>().StartTimer();
    }

    public void GameOver()
    {
        GameData.inGame = false;
        FindObjectOfType<AudioManager>().PlayOneShot("GameOver", GameData.GetSFXVolume());
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
