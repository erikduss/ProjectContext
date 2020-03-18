using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplay : MonoBehaviour {

	public Spell spell;

	public TextMesh nameText;
	public TextMesh descriptionText;

	public SpriteRenderer artworkImage;

	public TextMesh manaText;

    public float textSize = 1;

	// Use this for initialization
	void Start () {
		nameText.text = spell.name;
        nameText.characterSize = textSize;

		descriptionText.text = spell.description;

		artworkImage.sprite = spell.artwork;

		manaText.text = spell.manaCost.ToString();
	}
	
}
