using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pochita", menuName = "Kaiju", order = 1)]
public class Kaiju : ScriptableObject
{
	public int id = 0;
	public float maxHunger = 100f;
	public float hungerDecay = 10f;

	public float maxHealth = 100f;
	public float healthDecayFromHunger = 100f;

	public float maxDestructionNeed = 100f;
	public float destructionDecay = 20f;

	public Sprite sprite;
	public Vector3 hatPoint;
	public string typeName;

}
