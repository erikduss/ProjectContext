using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class minionController : MonoBehaviour
{

    private Vector3 small = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 big = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 startPos;
    private Vector3 expandPos;

    public bool isOpponentCard = false;

    public Card thisCard;

    public SpriteRenderer cardBackground;
    public SpriteRenderer cardImage;
    public SpriteRenderer cardFrame;
    public SortingGroup cardName;
    public SortingGroup cardDescription;
    public SortingGroup cardMana;
    public SortingGroup cardAttack;
    public SortingGroup cardHealth;

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

        switch (thisCard.effect)
        {
            case Card.Effect.Destroy_All_Minions:
                    if (!isOpponentCard && gameManager.aliveMinionsMainPlayer < gameManager.maxBoardSize)
                    {
                        gameManager.destroyAllMinions();
                        gameManager.SummonMinion("Inner_Demon");
                        gameManager.cardsInHand.Remove(this.gameObject);
                        gameManager.amountOfCardsInHand--;
                        gameManager.rearrangeCards(1);
                        Destroy(this.gameObject);
                    }
                    else if (!isOpponentCard && gameManager.aliveMinionsOpponent < gameManager.maxBoardSize)
                    {
                        gameManager.destroyAllMinions();
                        gameManager.SummonMinion("Inner_Demon");
                        gameManager.cardsInOpponentHand.Remove(this.gameObject);
                        gameManager.opponentCardsInHand--;
                        gameManager.rearrangeCards(2);
                        Destroy(this.gameObject);
                    }
                    else
                    {

                    }
                break;
            case Card.Effect.Reduce_Question:
                break;
            case Card.Effect.Destroy:
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
        cardAttack.sortingOrder = 7;
        cardHealth.sortingOrder = 7;

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
        cardAttack.sortingOrder = 3;
        cardHealth.sortingOrder = 3;

        madeBig = false;
    }
}
