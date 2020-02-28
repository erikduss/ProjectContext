using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class spellController : MonoBehaviour
{

    private Vector3 small = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 big = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 startPos;
    private Vector3 expandPos;


    public SpriteRenderer cardBackground;
    public SpriteRenderer cardImage;
    public SpriteRenderer cardFrame;
    public SortingGroup cardName;
    public SortingGroup cardDescription;
    public SortingGroup cardMana;

    private bool grabbedCard = false;
    private bool madeBig = false;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.localPosition;
        expandPos = new Vector3(startPos.x, startPos.y + 3, startPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedCard)
        {
            if (!madeBig)
            {
                makeCardBig();
            }
            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.Lerp(transform.position, mouseposition, 0.2f);
        }
    }

    void OnMouseEnter()
    {
        if (grabbedCard) return;
        
        this.transform.localPosition = expandPos;

        makeCardBig();
    }

    void OnMouseExit()
    {
        if (grabbedCard) return;
        this.transform.localPosition = startPos;

        makeCardSmall();
    }

    private void OnMouseDown()
    {
        if(!grabbedCard)
        {
            GrabCard();
        }
        else
        {
            releaseCard();
        }
    }

    private void GrabCard()
    {
        grabbedCard = true;
    }

    private void releaseCard()
    {
        grabbedCard = false;

        gameManager.SummonMinion("Spawned_Creature");

        Destroy(this.gameObject);
       //this.transform.localPosition = startPos;
        //makeCardSmall();
    }

    private void makeCardBig()
    {
        this.transform.localScale = big;

        cardBackground.sortingOrder = 4;
        cardImage.sortingOrder = 5;
        cardFrame.sortingOrder = 6;
        cardName.sortingOrder = 7;
        cardDescription.sortingOrder = 7;
        cardMana.sortingOrder = 7;

        madeBig = true;
    }

    private void makeCardSmall()
    {
        this.transform.localScale = small;

        cardBackground.sortingOrder = 0;
        cardImage.sortingOrder = 1;
        cardFrame.sortingOrder = 2;
        cardName.sortingOrder = 3;
        cardDescription.sortingOrder = 3;
        cardMana.sortingOrder = 3;

        madeBig = false;
    }
}
