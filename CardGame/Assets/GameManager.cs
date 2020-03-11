using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playBoard;

    public List<GameObject> minionsToSpawnOnField;
    public List<Card> minionsData;

    public List<GameObject> playableCards;
    private List<GameObject> opponentDeck;

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
    public int aliveMinionsMainPlayer = 0;
    public int aliveMinionsOpponent = 0;

    public List<BoardSpot> mainPlayerBoard = new List<BoardSpot>();
    public List<BoardSpot> opponentBoard = new List<BoardSpot>();

    private List<int> minionXPos = new List<int> {-12, -8, -4, 0, 4, 8, 12 };
    private List<int> cardsInHandXPos = new List<int> { -12, -8, -4, 0, 4, 8, 12 };

    private int startingCards = 3;
    public int amountOfCardsInHand = 0;
    private int maxHandSize = 7;

    public int opponentCardsInHand = 0;

    public List<GameObject> cardsInHand = new List<GameObject>();
    public List<GameObject> cardsInOpponentHand = new List<GameObject>();

    public int maxBoardSize = 7;

    private int spawnedMinions = 0;

    public bool pauseInteractions = false;

    private int currentMana = 0;
    private int maxManaThisTurn = 0;
    private int currentTurn = 0;
    private int maxMana = 10;

    public Text manaText;
    public Text cardsText;
    public Text playerText;

    private int playerTurn = 1;

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

        opponentDeck = new List<GameObject>(playableCards);

        List<int> alreadyPickedCards = new List<int>();

        for(int i = 0; i<= startingCards; i++)
        {
            drawCard(1);
            drawCard(2);
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
        int listID = 0;

        if(playerTurn == 1)
        {
            listID = mainPlayerBoard.FindIndex(a => a.Occupied == false);
        }
        else
        {
            listID = opponentBoard.FindIndex(a => a.Occupied == false);
        }

        GameObject minionToSpawn = minionsToSpawnOnField.Find(x => x.name == minionName);
        Card minionData = minionsData.Find(x => x.objectName == minionName);

        int minionX = minionXPos[listID];

        alivePlayerMinions++;
        spawnedMinions++;

        minionToSpawn.GetComponent<minionBehavior>().minionID = spawnedMinions; //give the minion an unique ID
        minionToSpawn.GetComponent<minionBehavior>().health = minionData.health;
        minionToSpawn.GetComponent<minionBehavior>().played = true;

        if (playerTurn == 1)
        {
            mainPlayerBoard[listID].placeMinion(minionData.attack, minionData.health, alivePlayerMinions); //Add the minion data to the board
        }
        else
        {
            opponentBoard[listID].placeMinion(minionData.attack, minionData.health, alivePlayerMinions); //Add the minion data to the board
        }

        activeMinions.Add(minionToSpawn);

        if (playerTurn == 1)
        {
            GameObject minion = Instantiate(minionToSpawn, new Vector3(minionX, -2.5f, 0), Quaternion.identity); //Spawn minion
            aliveMinionsMainPlayer++;
            minion.transform.parent = playBoard.transform;
        }
        else
        {
            GameObject minion = Instantiate(minionToSpawn, new Vector3(minionX, -2.5f, 0), new Quaternion(0,0,0,0)); //Spawn minion
            aliveMinionsOpponent++;
            minion.transform.parent = playBoard.transform;
        } 
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
        aliveMinionsOpponent = 0;
        aliveMinionsMainPlayer = 0;
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
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }
        else
        {
            playerTurn = 1;
            if (currentTurn < maxMana) currentTurn++;
        }

        playerText.text = "Turn: Player " + playerTurn;

        drawCard(playerTurn);
        setTurn(currentTurn);
       
        int rotation = 0;
        if (playBoard.transform.rotation.x == 0) rotation = 180;
        playBoard.transform.rotation = new Quaternion(rotation, 0, 0, 0);
    }

    public void drawCard(int player)
    {
        if(player == 1)
        {
            if (amountOfCardsInHand < maxHandSize)
            {
                int cardNumber = Random.Range(0, playableCards.Count);
                GameObject playerCard = Instantiate(playableCards[cardNumber], new Vector3(cardsInHandXPos[amountOfCardsInHand], -7.5f, 0), new Quaternion(0, 0, 0, 0)); //Spawn card in hand
                playerCard.transform.parent = playBoard.transform;
                playerCard.transform.rotation = new Quaternion(0, 0, 0, 0);
                playerCard.transform.localPosition = new Vector3(cardsInHandXPos[amountOfCardsInHand], -7.5f, 0);
                playableCards.Remove(playableCards[cardNumber]);
                amountOfCardsInHand++;
                cardsText.text = "Cards left: " + playableCards.Count;
                cardsInHand.Add(playerCard);
            }
        }
        else
        {
            if (opponentCardsInHand < maxHandSize)
            {
                int cardNumber = Random.Range(0, opponentDeck.Count);
                GameObject playerCard = Instantiate(opponentDeck[cardNumber], new Vector3(cardsInHandXPos[opponentCardsInHand], 7.5f, 0), new Quaternion(180,0,0,0)); //Spawn card in hand
                playerCard.transform.parent = playBoard.transform;
                playerCard.transform.rotation = new Quaternion(180, 0, 0, 0);
                playerCard.transform.position = new Vector3(cardsInHandXPos[opponentCardsInHand], 7.5f, 0);
                opponentDeck.Remove(opponentDeck[cardNumber]);
                opponentCardsInHand++;
                cardsText.text = "Cards left: " + opponentDeck.Count;
                cardsInOpponentHand.Add(playerCard);

                if (playerCard.tag == "Minion")
                {
                    playerCard.GetComponent<minionController>().isOpponentCard = true;
                }
                else if (playerCard.tag == "Spell")
                {
                    playerCard.GetComponent<spellController>().isOpponentCard = true;
                }
            }
        }
    }

    public void rearrangeCards(int player)
    {
        if(player == 1)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                cardsInHand[i].transform.position = new Vector3(cardsInHandXPos[i], cardsInHand[i].transform.position.y, cardsInHand[i].transform.position.z);
            }
        }
        else
        {
            for (int i = 0; i < cardsInOpponentHand.Count; i++)
            {
                cardsInOpponentHand[i].transform.position = new Vector3(cardsInHandXPos[i], cardsInOpponentHand[i].transform.position.y, cardsInOpponentHand[i].transform.position.z);
            }
        }
    }
}
