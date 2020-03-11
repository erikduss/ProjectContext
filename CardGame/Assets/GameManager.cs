using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> minionsToSpawnOnField;
    public List<Card> minionsData;

    public List<GameObject> playableCards;

    public GameObject questionCreationPanel;
    public Button btn_submitQuestion;

    private Question mainPlayerQuestion = new Question();

    public Text question;
    public Text fake_answer_1;
    public Text fake_answer_2;
    public Text correct_answer;

    public Text errorMessage;

    public List<GameObject> activeMinions = new List<GameObject>();

    public int alivePlayerMinions = 0;

    public List<BoardSpot> mainPlayerBoard = new List<BoardSpot>();
    public List<BoardSpot> opponentBoard = new List<BoardSpot>();

    private List<int> minionXPos = new List<int> {-12, -8, -4, 0, 4, 8, 12 };
    private List<int> cardsInHandXPos = new List<int> { -12, -8, -4, 0, 4, 8, 12 };

    private int startingCards = 3;
    private int amountOfCardsInHand = 0;
    private int maxHandSize = 7;

    private List<GameObject> cardsInHand = new List<GameObject>();

    public int maxBoardSize = 7;

    private int spawnedMinions = 0;

    public bool pauseInteractions = false;

    private int currentMana = 0;
    private int maxManaThisTurn = 0;
    private int currentTurn = 0;
    private int maxMana = 10;

    public Text manaText;
    public Text cardsText;

    // Start is called before the first frame update
    void Start()
    {
        questionCreationPanel.SetActive(false);
        btn_submitQuestion.onClick.AddListener(submitQuestion);
        for(int i = 0; i< maxBoardSize; i++)
        {
            mainPlayerBoard.Add(new BoardSpot(i));
            opponentBoard.Add(new BoardSpot(i));
        }

        List<int> alreadyPickedCards = new List<int>();

        for(int i = 0; i<= startingCards; i++)
        {
            int cardNumber = Random.Range(0, playableCards.Count);
            if (alreadyPickedCards.Contains(cardNumber))
            {
                cardNumber = Random.Range(0, playableCards.Count);
                if (alreadyPickedCards.Contains(cardNumber))
                {
                    cardNumber = Random.Range(0, playableCards.Count);
                    if (alreadyPickedCards.Contains(cardNumber))
                    {
                        cardNumber = Random.Range(0, playableCards.Count);
                        if (alreadyPickedCards.Contains(cardNumber))
                        {
                            cardNumber = Random.Range(0, playableCards.Count);
                            Debug.Log("TRIED TO GET MORE CARDS BUT FAILED");
                        }
                    }
                }
            }
            cardsInHand.Add(playableCards[cardNumber]);
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            playableCards.Remove(cardsInHand[i]);
            Instantiate(cardsInHand[i], new Vector3(cardsInHandXPos[amountOfCardsInHand], -7.5f, 0), Quaternion.identity); //Spawn card in hand
            amountOfCardsInHand++;
        }
        setTurn(1);
        cardsText.text = "Cards left: " + playableCards.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonMinion(string minionName)
    {
        int listID = mainPlayerBoard.FindIndex(a => a.Occupied == false);

        GameObject minionToSpawn = minionsToSpawnOnField.Find(x => x.name == minionName);
        Card minionData = minionsData.Find(x => x.objectName == minionName);

        int minionX = minionXPos[alivePlayerMinions];

        alivePlayerMinions++;
        spawnedMinions++;

        minionToSpawn.GetComponent<minionBehavior>().minionID = spawnedMinions; //give the minion an unique ID
        minionToSpawn.GetComponent<minionBehavior>().health = minionData.health;
        minionToSpawn.GetComponent<minionBehavior>().played = true;

        mainPlayerBoard[listID].placeMinion(minionData.attack, minionData.health, alivePlayerMinions); //Add the minion data to the board

        activeMinions.Add(minionToSpawn);

        Instantiate(minionToSpawn, new Vector3(minionX, 0, 0), Quaternion.identity); //Spawn minion
    }

    public void destroyAllMinions()
    {
        foreach(BoardSpot spot in mainPlayerBoard)
        {
            if (spot.Occupied)
            {
                spot.killMinion();
            }
        }
        foreach (BoardSpot spot in opponentBoard)
        {
            if (spot.Occupied)
            {
                spot.killMinion();
            }
        }

        List<GameObject> minionsToKill = new List<GameObject>();
        minionsToKill.AddRange(GameObject.FindGameObjectsWithTag("PlayedMinion"));

        foreach (GameObject minion in minionsToKill)
        {
            //Destroy(minion);
            minion.GetComponent<minionBehavior>().health = 0;
            //activeMinions.Remove(minion);
        }

        activeMinions.Clear();
        alivePlayerMinions = 0;
    }

    public void setQuestion()
    {
        pauseInteractions = true;
        questionCreationPanel.SetActive(true);
    }

    public void submitQuestion()
    {
        if(question.text.Length > 0 && fake_answer_1.text.Length > 0 && fake_answer_2.text.Length > 0 && correct_answer.text.Length > 0)
        {
            pauseInteractions = false;
            mainPlayerQuestion.question = question.text;
            mainPlayerQuestion.fakeAnswer_1 = fake_answer_1.text;
            mainPlayerQuestion.fakeAnswer_2 = fake_answer_2.text;
            mainPlayerQuestion.correctAnswer = correct_answer.text;
            questionCreationPanel.SetActive(false);
        }
        else
        {
            errorMessage.text = "Not all required information has been filled in";
        }
    }

    public void damageMinion()
    {

    }

    public void damagePlayer()
    {

    }

    public void healMinion()
    {

    }

    public void healPlayer()
    {

    }

    private void setTurn(int mana)
    {
        currentMana = mana;
        currentTurn = mana;
        maxManaThisTurn = mana;
        manaText.text = "Mana: " + currentMana + "/" + maxManaThisTurn;
    }

    public void endTurn()
    {
        if(currentTurn < maxMana) currentTurn++;
        setTurn(currentTurn);
        drawCard();
    }

    public void drawCard()
    {
        if(amountOfCardsInHand < maxHandSize)
        {
            int cardNumber = Random.Range(0, playableCards.Count);
            Instantiate(playableCards[cardNumber], new Vector3(cardsInHandXPos[amountOfCardsInHand], -7.5f, 0), Quaternion.identity); //Spawn card in hand
            playableCards.Remove(playableCards[cardNumber]);
            amountOfCardsInHand++;
            cardsText.text = "Cards left: " + playableCards.Count;
        }
    }
}
