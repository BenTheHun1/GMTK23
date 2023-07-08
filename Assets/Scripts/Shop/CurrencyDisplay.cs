using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    private TextMeshProUGUI currencyText;

    // Start is called before the first frame update
    void Start()
    {
        currencyText = GetComponent<TextMeshProUGUI>();
        RefreshCurrency();
    }

    public void RefreshCurrency() => currencyText.text = GameData.currency.ToString("n0");
}
