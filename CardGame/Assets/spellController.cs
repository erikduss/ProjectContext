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

    public Spell thisSpell;

    public SpriteRenderer cardBackground;
    public SpriteRenderer cardImage;
    public SpriteRenderer cardFrame;
    public SortingGroup cardName;
    public SortingGroup cardDescription;
    public SortingGroup cardMana;

    private bool grabbedCard = false;
    private bool madeBig = false;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        startPos = this.transform.localPosition;
        expandPos = new Vector3(startPos.x, startPos.y + 3, startPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.pauseInteractions) return;

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
        if (gameManager.pauseInteractions) return;
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
        if (gameManager.pauseInteractions) return;
        if (!grabbedCard)
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

        switch (thisSpell.effect)
        {
            case Spell.Effect.Summon:
                    if (gameManager.alivePlayerMinions < gameManager.maxBoardSize)
                    {
                        for (var i = 0; i < thisSpell.Value; i++)
                        {
                            if (gameManager.alivePlayerMinions < gameManager.maxBoardSize)
                            {
                                gameManager.SummonMinion("Spawned_Creature");
                            }
                        }
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        //THE PLAYER DOES NOT HAVE ENOUGH BOARD SPACE TO USE THIS SPELL.
                    }
                break;
            case Spell.Effect.Damage_All_Enemies:
                break;
            case Spell.Effect.Damage_Single:
                break;
            case Spell.Effect.Detroy_Minion:
                break;
            case Spell.Effect.Heal_Friendly:
                break;
            case Spell.Effect.Heal_Single:
                break;
            case Spell.Effect.Set_Question:
                gameManager.setQuestion();
                Destroy(this.gameObject);
                break;
            case Spell.Effect.Take_Control:
                break;
            default:
                break;
        }
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
