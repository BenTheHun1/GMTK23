using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KaijuStats : MonoBehaviour
{
	[Tooltip("The initial Kaiju.")] public Kaiju startingKaiju;
	[Tooltip("The current Kaiju.")] public Kaiju kaiju;
	[Tooltip("All herbivore Kaiju")] public Kaiju[] herbKaiju;
	[Tooltip("All omniivore Kaiju")] public Kaiju[] omniKaiju;
	[Tooltip("All carnivore Kaiju")] public Kaiju[] carnKaiju;
	public int kaijuLvl;

	[Tooltip("Stats")] public float hunger, health, destructionNeed;
	private Image hungerDisplay, healthDisplay, destructionDisplay;

	[Tooltip("The Kaiju's Name.")] public string Name; //Add way to display and edit
	[Tooltip("The Kaiju's ID")] public int monsterTypeID;
	[Tooltip("Current alignment. Positive is carnivore, negative is herbivore.")] public int alignment;


	private SpriteRenderer hatObject;
	[Tooltip("Current equipped Hat.")] public Hat curHat;

	public AppleMinigame appleMiniGame;
	public RatMinigame ratMiniGame;


	// Start is called before the first frame update
	void Start()
	{
		if (appleMiniGame)
		{
			appleMiniGame.gameObject.SetActive(false);
		}
		if (ratMiniGame)
		{
			ratMiniGame.gameObject.SetActive(false);
		}

		hungerDisplay = GameObject.Find("Hunger Meter").GetComponent<Image>();
		healthDisplay = GameObject.Find("Health Meter").GetComponent<Image>();
		destructionDisplay = GameObject.Find("Destruction Meter").GetComponent<Image>();
		
		hatObject = transform.GetChild(0).GetComponent<SpriteRenderer>();
		hatObject.color = new Color(0, 0, 0, 0);

		hunger = startingKaiju.maxHunger; //Otherwise starts at 0
		health = startingKaiju.maxHealth;
		destructionNeed = startingKaiju.maxDestructionNeed;

		LoadKaiju(startingKaiju);
	}

	public void LoadHat(Hat newHat)
	{
		curHat = newHat;
		GameData.currentEquipment = curHat;

		hatObject.sprite = curHat.sprite;
		hatObject.color = Color.white;

		hatObject.transform.localPosition = kaiju.hatPoint;

	}

	void LoadKaiju(Kaiju newKaiju)
	{
		kaiju = newKaiju;

		GetComponent<SpriteRenderer>().sprite = kaiju.sprite;

		if (hunger > kaiju.maxHunger) //If its bigger than the max we want to reduce it to the max value
		{
			hunger = kaiju.maxHunger;
		}

		if (health > kaiju.maxHealth)
		{
			health = kaiju.maxHealth;
		}

		if (destructionNeed > kaiju.maxDestructionNeed)
		{
			destructionNeed = kaiju.maxDestructionNeed;
		}

		if (curHat)
		{
			LoadHat(curHat); //to ensure the hat is positioned correctly
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (GameData.inGame)
        {
			if (hunger > 0f)
			{
				hunger -= 0.1f * Time.deltaTime * kaiju.hungerDecay;
				hungerDisplay.fillAmount = hunger / kaiju.maxHunger;
				hunger = Mathf.Clamp(hunger, 0f, kaiju.maxHunger);
			}
			else
			{
				health -= 0.1f * Time.deltaTime * kaiju.healthDecayFromHunger;
				healthDisplay.fillAmount = health / kaiju.maxHealth;
				health = Mathf.Clamp(health, 0f, kaiju.maxHealth);
			}

			destructionNeed -= 0.1f * Time.deltaTime * kaiju.destructionDecay;
			destructionNeed = Mathf.Clamp(destructionNeed, 0f, kaiju.maxDestructionNeed);

			destructionDisplay.fillAmount = destructionNeed / kaiju.maxDestructionNeed;

			if (health <= 0f)
			{
				GameManager.instance.GameOver();
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// Give the Kaiju an item and acts accordingly depending on the ID given.
	/// </summary>
	/// <param name="itemID">The ID for the item.</param>
	public void GiveKaijuItem(int itemID)
    {
        switch (itemID)
        {
			//Apple
			case 0:
				Feed(-4);
				break;
			//Human
			case 1:
				Feed(10);
				break;
			//Rat
			case 2:
				Feed(4);
				break;
		}
    }

	public void Feed(int amount)
	{
		hunger += Mathf.Abs(amount);
		alignment += amount;
		if (hunger > kaiju.maxHunger)
		{
			hunger = kaiju.maxHunger;
		}


		if(amount > 0)
			FindObjectOfType<AudioManager>().PlayOneShot("FeedCarnivore", GameData.GetSFXVolume());
		else
			FindObjectOfType<AudioManager>().PlayOneShot("FeedHerbivore", GameData.GetSFXVolume());
	}

	public void TriggerEvolution()
	{
		if (alignment > 25f)
		{
			if (kaiju.kaijuType == Kaiju.type.carnivore || kaiju.kaijuType == Kaiju.type.original)
			{
				kaijuLvl++;
			}

			LoadKaiju(carnKaiju[kaijuLvl - 1]);
		}
		else if (alignment < -25f)
		{
			if (kaiju.kaijuType == Kaiju.type.herbivore || kaiju.kaijuType == Kaiju.type.original)
			{
				kaijuLvl++;
			}

			LoadKaiju(herbKaiju[kaijuLvl - 1]);
		}
		else
		{
			if (kaiju.kaijuType == Kaiju.type.omnivore || kaiju.kaijuType == Kaiju.type.original)
			{
				kaijuLvl++;
			}

			LoadKaiju(omniKaiju[kaijuLvl - 1]);
		}

		alignment = 0;

		/*foreach (Kaiju k in allKaiju)
		{
			if (k.id == monsterTypeID)
			{
				LoadKaiju(k);
				break;
			}
		}*/
	}

	public void Destruction()
	{
		destructionNeed += 5f;
		if (destructionNeed > kaiju.maxDestructionNeed)
		{
			destructionNeed = kaiju.maxDestructionNeed;
		}
	}

	public void StartFruitGame(bool startIfTrue)
	{
		hungerDisplay.transform.parent.GetComponent<Canvas>().enabled = !startIfTrue;
		gameObject.GetComponent<SpriteRenderer>().enabled = !startIfTrue;
		hatObject.enabled = !startIfTrue;
		FindObjectOfType<ShopController>().GetComponent<Canvas>().enabled = !startIfTrue;
		FindObjectOfType<InventoryController>().GetComponent<Canvas>().enabled = !startIfTrue;

		appleMiniGame.gameObject.SetActive(startIfTrue);
		if (startIfTrue)
		{
			FindObjectOfType<AppleMinigameControls>().GetComponent<SpriteRenderer>().sprite = kaiju.sprite;
			FindObjectOfType<AppleMinigameControls>().speed = kaiju.speed;
			appleMiniGame.StartMinigame();
		}

	}

	public void StartMeatGame(bool startIfTrue)
	{
		hungerDisplay.transform.parent.GetComponent<Canvas>().enabled = !startIfTrue;
		gameObject.GetComponent<SpriteRenderer>().enabled = !startIfTrue;
		hatObject.enabled = !startIfTrue;
		FindObjectOfType<ShopController>().GetComponent<Canvas>().enabled = !startIfTrue;
		FindObjectOfType<InventoryController>().GetComponent<Canvas>().enabled = !startIfTrue;

		ratMiniGame.gameObject.SetActive(startIfTrue);
		if (startIfTrue)
		{
			ratMiniGame.StartMinigame();
		}
	}
}
