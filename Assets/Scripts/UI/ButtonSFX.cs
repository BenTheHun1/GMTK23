using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private string buttonHover = "ButtonHover";
    [SerializeField] private string buttonClick = "ButtonClick";

    private EventTrigger buttonEvents;

    // Start is called before the first frame update
    void Start()
    {
        buttonEvents = GetComponent<EventTrigger>();

        EventTrigger.Entry selectEntry = new EventTrigger.Entry();
        selectEntry.eventID = EventTriggerType.Select;
        selectEntry.callback.AddListener( (eventData) => { SFXOnSelect(); } );
        buttonEvents.triggers.Add( selectEntry );

        EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener((eventData) => { SFXOnSelect(); });
        buttonEvents.triggers.Add(hoverEntry);

        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((eventData) => { SFXOnClick(); });
        buttonEvents.triggers.Add(clickEntry);
    }

    private void SFXOnSelect()
    {
        if(!GameData.transitionActive)
            FindObjectOfType<AudioManager>().PlayOneShot(buttonHover, GameData.GetSFXVolume());
    }

    private void SFXOnClick()
    {
        if(!GameData.transitionActive)
            FindObjectOfType<AudioManager>().PlayOneShot(buttonClick, GameData.GetSFXVolume());
    }
}
