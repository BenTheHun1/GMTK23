using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private RectTransform optionsTransform;
    [SerializeField] private float optionsMenuStartPos;
    [SerializeField] private float optionsMenuEndPos;
    [SerializeField] private float slideAnimationDuration;
    [SerializeField] private LeanTweenType slideAnimationEaseType;

    private bool optionsMenuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMenu();   
    }

    public void ToggleOptionsMenu()
    {
        optionsMenuActive = !optionsMenuActive;

        GameData.transitionActive = true;

        LeanTween.moveX(optionsTransform, optionsMenuActive? optionsMenuEndPos: optionsMenuStartPos, slideAnimationDuration).setEase(slideAnimationEaseType)
            .setOnComplete(() => GameData.transitionActive = false);
    }

    private void InitializeMenu()
    {
        musicSlider.value = GameData.GetBGMVolume() / 0.1f;
        sfxSlider.value = GameData.GetSFXVolume() / 0.1f;
    }

    public void SetMusicVolume(float newVol)
    {
        PlayerPrefs.SetFloat("BGMVolume", newVol * 0.1f);
        if(GameData.currentMusicPlaying != "")
            FindObjectOfType<AudioManager>().ChangeVolume(GameData.currentMusicPlaying, GameData.GetBGMVolume());
    }

    public void SetSFXVolume(float newVol)
    {
        PlayerPrefs.SetFloat("SFXVolume", newVol * 0.1f);
    }
}
