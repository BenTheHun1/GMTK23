using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KaijuStats : MonoBehaviour
{
	public Kaiju startingKaiju;
	public Kaiju kaiju;
	public float hunger, health, destructionNeed;
	public Image hungerDisplay, healthDisplay, destructionDisplay;

	public string Name;
	public int monsterTypeID; //use to display the right sprite?
	public int alignment; //positive is carnivore, negative is herbivore

	public Kaiju[] allKaiju;

    // Start is called before the first frame update
    void Start()
    {
		hungerDisplay = GameObject.Find("Hunger Meter").GetComponent<Image>();
		healthDisplay = GameObject.Find("Health Meter").GetComponent<Image>();
		destructionDisplay = GameObject.Find("Destruction Meter").GetComponent<Image>();

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
	}

    // Update is called once per frame
    void Update()
    {
		if (hunger > 0f)
		{
			hunger -= 0.1f * Time.deltaTime * kaiju.hungerDecay;
			hungerDisplay.fillAmount = hunger / 100f;
		}
		else
		{
			health -= 0.1f * Time.deltaTime * kaiju.healthDecayFromHunger;
			healthDisplay.fillAmount = health / 100f;
		}

		destructionNeed -= 0.1f * Time.deltaTime * kaiju.destructionDecay;
		destructionDisplay.fillAmount = destructionNeed / 100f;

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
		if (alignment >= 0)
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
			}
		}
	}

}
