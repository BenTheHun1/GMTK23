using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FashionShow : MonoBehaviour
{
	public Button StartFashionShow;
	public float curThreshold;
	private KaijuStats kaiju;

    // Start is called before the first frame update
    void Start()
    {
		StartFashionShow.interactable = false;
		kaiju = FindObjectOfType<KaijuStats>();
		curThreshold = 5;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MakeFashionShowAvailable()
	{
		StartFashionShow.interactable = true;
	}

	public void StartShow()
	{
		float sexy = kaiju.kaiju.sexiness + kaiju.curHat.sexiness;
		if (sexy >= curThreshold)
		{
			//1st place
			GameData.currency += 10000;
		}
		else if (sexy >= curThreshold * 0.9f)
		{
			//2nd place
			GameData.currency += 7500;
		}
		else if (sexy >= curThreshold * 0.8f)
		{
			//3rd place
			GameData.currency += 5000;
		}
	}
}
