using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

	public Card card;

	public TextMesh nameText;
	public TextMesh descriptionText;

	public SpriteRenderer artworkImage;

	public TextMesh manaText;
	public TextMesh attackText;
	public TextMesh healthText;

    public float textSize = 1;

	// Use this for initialization
	void Start () {
		nameText.text = card.name;
        nameText.characterSize = textSize;

		descriptionText.text = card.description;

		artworkImage.sprite = card.artwork;

		manaText.text = card.manaCost.ToString();
		attackText.text = card.attack.ToString();
		healthText.text = card.health.ToString();
	}
	
}
