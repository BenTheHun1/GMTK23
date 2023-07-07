using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaijuStats : MonoBehaviour
{
	public float hunger;
	public Image hungerDisplay;
	public float hungerDecay;

	public float health;
	public Image healthDisplay;
	public float healthDecayFromHunger;

	public float destructionNeed;
	public Image destructionDisplay;
	public float destructionDecay;

	public string Name;
	public int monsterTypeID; //use to display the right sprite?
	public int alignment; //positive is carnivore, negative is herbivore

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (hunger > 0f)
		{
			hunger -= 0.1f * Time.deltaTime * hungerDecay;
			hungerDisplay.fillAmount = hunger / 100f;
		}
		else
		{
			health -= 0.1f * Time.deltaTime * healthDecayFromHunger;
			healthDisplay.fillAmount = health / 100f;
		}

		destructionNeed -= 0.1f * Time.deltaTime * destructionDecay;
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
		if (hunger > 100f)
		{
			hunger = 100f;
		}
	}

	public void TriggerEvolution()
	{
		if (alignment >= 0f)
		{
			monsterTypeID++;
		}
		if (alignment < 0f)
		{
			monsterTypeID--;
		}
	}

}
