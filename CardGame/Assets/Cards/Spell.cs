using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject {

	public new string name;
    [TextArea]
    public string description;

	public Sprite artwork;

	public int manaCost;

	public void Print ()
	{
		Debug.Log(name + ": " + description + " The card costs: " + manaCost);
	}

}
