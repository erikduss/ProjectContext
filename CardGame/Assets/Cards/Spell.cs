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

    public enum Effect { Summon, Set_Question, Detroy_Minion, Destroy_All_Minions, Reduce_Fake_Answers }

    public Effect effect;

    public int Value;

    public void Print ()
	{
		Debug.Log(name + ": " + description + " The card costs: " + manaCost);
	}

}
