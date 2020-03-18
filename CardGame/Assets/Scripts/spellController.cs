using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class spellController : MonoBehaviour
{

    private Vector3 small = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 big = new Vector3(0.5f, 0.5f, 0.5f);

    public Vector3 startPos;
    private Vector3 expandPos;

    public bool isOpponentCard = false;

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
        setNewPos(this.transform.localPosition);
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

    public void setNewPos(Vector3 pos)
    {
        startPos = pos;
        if (!isOpponentCard)
        {
            expandPos = new Vector3(startPos.x, startPos.y + 3, startPos.z);
        }
        else
        {
            expandPos = new Vector3(startPos.x, startPos.y - 3, startPos.z);
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
                    if (!isOpponentCard && gameManager.aliveMinionsMainPlayer < gameManager.maxBoardSize)
                    {
                        for (var i = 0; i < thisSpell.Value; i++)
                        {
                            if (gameManager.aliveMinionsMainPlayer < gameManager.maxBoardSize)
                            {
                                gameManager.SummonMinion("Spawned_Creature");
                            }
                        }
                        gameManager.cardsInHand.Remove(this.gameObject);
                        gameManager.amountOfCardsInHand--;
                        gameManager.rearrangeCards(1);
                        Destroy(this.gameObject);
                    }
                    else if(isOpponentCard && gameManager.aliveMinionsOpponent < gameManager.maxBoardSize)
                    {
                        for (var i = 0; i < thisSpell.Value; i++)
                        {
                            if (gameManager.aliveMinionsOpponent < gameManager.maxBoardSize)
                            {
                                gameManager.SummonMinion("Spawned_Creature");
                            }
                        }
                        gameManager.cardsInOpponentHand.Remove(this.gameObject);
                        gameManager.opponentCardsInHand--;
                        gameManager.rearrangeCards(2);
                        Destroy(this.gameObject);
                    }
                    else
                    {

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
                if (!isOpponentCard)
                {
                    gameManager.cardsInHand.Remove(this.gameObject);
                    gameManager.amountOfCardsInHand--;
                    gameManager.rearrangeCards(1);
                }
                if (isOpponentCard)
                {
                    gameManager.cardsInOpponentHand.Remove(this.gameObject);
                    gameManager.opponentCardsInHand--;
                    gameManager.rearrangeCards(2);
                }
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
