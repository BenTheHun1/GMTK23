using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("GameMusic", GameData.GetBGMVolume());
        FindObjectOfType<TriggeredTimer>().InitializeTimer(3600f);
    }

    public void Init()
    {
        GameData.inGame = true;
        FindObjectOfType<TriggeredTimer>().StartTimer();
    }

    public void GameOver()
    {

    }

}
