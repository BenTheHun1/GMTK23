using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KaijuStats : MonoBehaviour
{
	[Tooltip("The initial Kaiju.")] public Kaiju startingKaiju;
	[Tooltip("The current Kaiju.")] public Kaiju kaiju;
	[Tooltip("All possible Kaiju (with different IDs).")] public Kaiju[] allKaiju;

	[Tooltip("Stats")] public float hunger, health, destructionNeed;
	private Image hungerDisplay, healthDisplay, destructionDisplay;

	[Tooltip("The Kaiju's Name.")] public string Name; //Add way to display and edit
	[Tooltip("The Kaiju's ID")] public int monsterTypeID;
	[Tooltip("Current alignment. Positive is carnivore, negative is herbivore.")] public int alignment;


	private SpriteRenderer hatObject;
	[Tooltip("Current equipped Hat.")] public Hat curHat;
	[Tooltip("All possible equiqabble hats.")] public Hat[] allHats;


	// Start is called before the first frame update
	void Start()
    {
		hungerDisplay = GameObject.Find("Hunger Meter").GetComponent<Image>();
		healthDisplay = GameObject.Find("Health Meter").GetComponent<Image>();
		destructionDisplay = GameObject.Find("Destruction Meter").GetComponent<Image>();
		
		hatObject = transform.GetChild(0).GetComponent<SpriteRenderer>();

		hunger = startingKaiju.maxHunger; //Otherwise starts at 0
		health = startingKaiju.maxHealth;
		destructionNeed = startingKaiju.maxDestructionNeed;

		FindObjectOfType<Timer>().StartCountdown(new System.TimeSpan(1, 0, 0));
		LoadKaiju(startingKaiju);
	}

	void LoadHat(Hat newHat)
	{
		curHat = newHat;

		hatObject.sprite = curHat.sprite;

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
		if (hunger > 0f)
		{
			hunger -= 0.1f * Time.deltaTime * kaiju.hungerDecay;
			hungerDisplay.fillAmount = hunger / kaiju.maxHunger;
		}
		else
		{
			health -= 0.1f * Time.deltaTime * kaiju.healthDecayFromHunger;
			healthDisplay.fillAmount = health / kaiju.maxHealth;
		}

		destructionNeed -= 0.1f * Time.deltaTime * kaiju.destructionDecay;
		destructionDisplay.fillAmount = destructionNeed / kaiju.maxDestructionNeed;

		if (health <= 0f)
		{
			Destroy(gameObject);
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
	}

	public void TriggerEvolution()
	{
		if (alignment > 0)
		{
			monsterTypeID++;
		}
		if (alignment < 0)
		{
			monsterTypeID--;
		}
		alignment = 0;
		foreach (Kaiju k in allKaiju)
		{
			if (k.id == monsterTypeID)
			{
				LoadKaiju(k);
				break;
			}
		}
	}

	public void GiveHimDaHat() // Temp
	{
		LoadHat(allHats[0]);
	}



}
