using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FashionShowController : MonoBehaviour
{
    [SerializeField] private RectTransform popupTransform;
    [SerializeField] private float popupStartPos;
    [SerializeField] private float popupEndPos;

    [SerializeField] private float popupEnterDuration;
    [SerializeField] private float popupExitDuration;

    [SerializeField] private LeanTweenType popupEnterEaseType;
    [SerializeField] private LeanTweenType popupExitEaseType;

    [SerializeField] private RectTransform fashionShowTransform;
    [SerializeField] private Vector2 fashionShowStartPos;
    [SerializeField] private Vector2 fashionShowEndPos;

    [SerializeField] private float fashionShowEnterDuration;
    [SerializeField] private float fashionShowExitDuration;

    [SerializeField] private LeanTweenType fashionShowEnterEaseType;
    [SerializeField] private LeanTweenType fashionShowExitEaseType;

    [SerializeField] private bool debugActivatePopup;

    private bool popupActive;
    private bool fashionShowActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (debugActivatePopup)
        {
            ActivatePopup();
            debugActivatePopup = false;
        }
    }

    /// <summary>
    /// Activates the popup for the fashion show.
    /// </summary>
    public void ActivatePopup()
    {
        LeanTween.moveX(popupTransform, popupEndPos, popupEnterDuration).setEase(popupEnterEaseType).setOnComplete(() => popupActive = true);
    }

    /// <summary>
    /// Hides the popup for the fashion show.
    /// </summary>
    public void DismissPopup()
    {
        LeanTween.moveX(popupTransform, popupStartPos, popupExitDuration).setEase(popupExitEaseType).setOnComplete(() => popupActive = false);
    }

    /// <summary>
    /// Starts the fashion show sequence.
    /// </summary>
    public void StartFashionShow()
    {
        DismissPopup();

        fashionShowTransform.localScale = Vector3.zero;
        LeanTween.moveX(fashionShowTransform, fashionShowEndPos.x, fashionShowEnterDuration).setEase(fashionShowEnterEaseType).setOnComplete(() => fashionShowActive = true);
        LeanTween.moveY(fashionShowTransform, fashionShowEndPos.y, fashionShowEnterDuration).setEase(fashionShowEnterEaseType);
        LeanTween.scale(fashionShowTransform, Vector3.one, fashionShowEnterDuration).setEase(fashionShowEnterEaseType).setOnComplete(() => FindObjectOfType<FashionShow>().StartShow());
		;
    }

	public void EndFashionShow()
	{
		LeanTween.moveX(fashionShowTransform, fashionShowStartPos.x, fashionShowEnterDuration).setEase(fashionShowEnterEaseType).setOnComplete(() => fashionShowActive = true);
		LeanTween.moveY(fashionShowTransform, fashionShowStartPos.y, fashionShowEnterDuration).setEase(fashionShowEnterEaseType);
		LeanTween.scale(fashionShowTransform, Vector3.zero, fashionShowEnterDuration).setEase(fashionShowEnterEaseType);
	}
}
