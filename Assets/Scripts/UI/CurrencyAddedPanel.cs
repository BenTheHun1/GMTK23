using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyAddedPanel : MonoBehaviour
{
    [SerializeField] private float enterDuration;
    [SerializeField] private float exitDelay;
    [SerializeField] private float exitDuration;
    [SerializeField] private LeanTweenType enterEaseType;
    [SerializeField] private LeanTweenType exitEaseType;

    [SerializeField] private TextMeshProUGUI currencyText;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        rectTransform.localScale = Vector3.zero;
    }

    public void ShowCurrencyAlert(int currencyAdded)
    {
        currencyText.text = GameData.kaijuName + " Found\n<size=50>" + currencyAdded.ToString("n0") + " Yen!</size>";

        LeanTween.scale(rectTransform.gameObject, Vector3.one, enterDuration).setEase(enterEaseType)
            .setOnComplete(() => LeanTween.delayedCall(exitDelay, () => LeanTween.scale(rectTransform.gameObject, Vector3.zero, exitDuration).setEase(exitEaseType)));
    }
}
