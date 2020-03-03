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

    public enum Effect { Summon, Damage_Single, Damage_All_Enemies, Heal_Single, Heal_Friendly, Set_Question, Take_Control, Detroy_Minion }

    public Effect effect;

    public int Value;

    public void Print ()
	{
		Debug.Log(name + ": " + description + " The card costs: " + manaCost);
	}

}
