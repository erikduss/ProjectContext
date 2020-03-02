using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject {

	public new string name;
    public new string objectName;
    [TextArea]
    public string description;

	public Sprite artwork;

	public int manaCost;
	public int attack;
	public int health;

    public enum Effect { None, Destroy, Destroy_All_Minions, Reduce_Question }

    public Effect effect;

    public int Value;

    public void Print ()
	{
		Debug.Log(name + ": " + description + " The card costs: " + manaCost);
	}

}
