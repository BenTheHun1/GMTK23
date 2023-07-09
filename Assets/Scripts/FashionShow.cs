using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FashionShow : MonoBehaviour
{
	public float curThreshold;
	private KaijuStats kaiju;

    // Start is called before the first frame update
    void Start()
    {
		kaiju = FindObjectOfType<KaijuStats>();
		curThreshold = 5;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MakeFashionShowAvailable()
	{
		FindObjectOfType<FashionShowController>().ActivatePopup();
	}

	public void StartShow()
	{
		float sexy = kaiju.kaiju.sexiness + (kaiju.curHat ? kaiju.curHat.sexiness : 0f);
		if (sexy >= curThreshold)
		{
			Debug.Log("You got first place!");
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 1000));
		}
		else if (sexy >= curThreshold * 0.9f)
		{
			Debug.Log("You got second place!");
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 750));
		}
		else if (sexy >= curThreshold * 0.8f) 
		{
			Debug.Log("You got third place!");
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 500));
		}
		else
		{
			Debug.Log("You lost...");
		}
		curThreshold += 5f;
	}
}
