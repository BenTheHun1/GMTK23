using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hat", menuName = "Hat", order = 2)]
public class Hat : ScriptableObject
{
	public new string name;
	public string description;

	public float sexiness = 0f;
	public int price;

	public Sprite sprite;

	public bool available = true;
	internal bool displayed = false;
	internal bool soldOut = false;
}
