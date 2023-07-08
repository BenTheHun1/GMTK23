using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KaijuStats : MonoBehaviour
{
	[Tooltip("The initial Kaiju.")] public Kaiju startingKaiju;
	[Tooltip("The current Kaiju.")] public Kaiju kaiju;
	[Tooltip("Stats")] public float hunger, health, destructionNeed;
	private Image hungerDisplay, healthDisplay, destructionDisplay;

	[Tooltip("The Kaiju's Name.")] public string Name; //Add way to display and edit
	[Tooltip("The Kaiju's ID")] public int monsterTypeID;
	[Tooltip("Current alignment. Positive is carnivore, negative is herbivore.")] public int alignment;

	[Tooltip("All possible Kaiju (with different IDs).")] public Kaiju[] allKaiju;

	[Tooltip("Current Hat.")] public GameObject curHat;

    // Start is called before the first frame update
    void Start()
    {
		hungerDisplay = GameObject.Find("Hunger Meter").GetComponent<Image>();
		healthDisplay = GameObject.Find("Health Meter").GetComponent<Image>();
		destructionDisplay = GameObject.Find("Destruction Meter").GetComponent<Image>();
		
		curHat = transform.GetChild(0).gameObject;

		hunger = startingKaiju.maxHunger;
		LoadKaiju(startingKaiju);
	}

	void LoadKaiju(Kaiju newKaiju)
	{
		kaiju = newKaiju;

		GetComponent<SpriteRenderer>().sprite = kaiju.sprite;

		if (hunger > kaiju.maxHunger)
		{
			hunger = kaiju.maxHunger;
		}

		health = kaiju.maxHealth;
		if (health > kaiju.maxHealth)
		{
			health = kaiju.maxHealth;
		}

		destructionNeed = kaiju.maxDestructionNeed;
		if (destructionNeed > kaiju.maxDestructionNeed)
		{
			destructionNeed = kaiju.maxDestructionNeed;
		}

		curHat.transform.localPosition = kaiju.hatPoint;
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

}
