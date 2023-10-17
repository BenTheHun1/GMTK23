using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FashionShow : MonoBehaviour
{
	public float curThreshold;
	private KaijuStats kaiju;
	public GameObject kaijuBody;
	public int performanceStep;
	public GameObject close;

	public TextMeshProUGUI curSexy, needSexy, Result;
	public Transform runwayL, runwayR;

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
		close.SetActive(false);
		kaijuBody.transform.localPosition = Vector3.zero;
		curSexy.text = "";
		needSexy.text = "";
		Result.text = "";

		kaijuBody.GetComponent<Image>().sprite = kaiju.kaiju.sprite;
		performanceStep = 7;
		MoveLeft();
	}

	public void MoveLeft()
	{
		if (performanceStep == 7)
		{
			LeanTween.moveX(kaijuBody, runwayL.position.x, 1f).setOnComplete(() => MoveRight());
			performanceStep--;
			ProceedPerformance();
		}
		else if (performanceStep > 2)
		{
			LeanTween.moveX(kaijuBody, runwayL.position.x, 2f).setOnComplete(() => MoveRight());
			performanceStep--;
			ProceedPerformance();
		}
	}

	public void MoveRight()
	{
		if (performanceStep > 2)
		{
			LeanTween.moveX(kaijuBody, runwayR.position.x, 2f).setOnComplete(() => MoveLeft());
			performanceStep--;
			ProceedPerformance();
		}

	}

	public void ProceedPerformance()
	{
		if (performanceStep == 5)
		{
			curSexy.text = "Sexiness: " + (kaiju.kaiju.sexiness + (kaiju.curHat ? kaiju.curHat.sexiness : 0f));
		}
		else if (performanceStep == 4)
		{
			needSexy.text = "Needed: " + (curThreshold);
		}
		else if (performanceStep == 3)
		{
			Result.text = CalcResult();
			close.SetActive(true);
		}
	}


	public string CalcResult()
	{
		float sexy = kaiju.kaiju.sexiness + (kaiju.curHat ? kaiju.curHat.sexiness : 0f);
		if (sexy >= curThreshold)
		{
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 1000));
			curThreshold += 5f; 
			return "1ST PLACE\nWon " + (int)((curThreshold - 5f) * 1000) + " Yen";
		}
		else if (sexy >= curThreshold * 0.9f)
		{
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 750));
			curThreshold += 5f;
			return "2ND PLACE\nWon " + (int)((curThreshold - 5f) * 7500) + " Yen";
		}
		else if (sexy >= curThreshold * 0.8f)
		{
			FindObjectOfType<ShopController>().UpdateCurrency((int)(curThreshold * 500));
			curThreshold += 5f;
			return "3RD PLACE\nWon " + (int)((curThreshold - 5f) * 500) + " Yen";
		}
		else
		{
			Debug.Log("You lost...");
			return "YOU LOST...";
		}
	}
}
