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

    private int amountOfAskedQuestions = 0;

    private List<Question> askedQuestions = new List<Question>();

    public InputField question;
    public InputField fake_answer_1;
    public InputField fake_answer_2;
    public InputField correct_answer;

    public GameObject questionPanel;
    public Button btn_submitAnswer;

    public Text askedQuestion;
    public Text answerField_1;
    public Text answerField_2;
    public Text answerField_3;

    private int currentQuestionID = 0;

    public int mainPlayerActiveQuestions = 0;
    public int opponentActiveQuestions = 0;

    public Text errorMessage;

    public List<GameObject> activeMinions = new List<GameObject>();

    public int alivePlayerMinions = 0;
    public int aliveMinionsMainPlayer = 0;
    public int aliveMinionsOpponent = 0;

    public List<BoardSpot> mainPlayerBoard = new List<BoardSpot>();
    public List<BoardSpot> opponentBoard = new List<BoardSpot>();

    private List<float> minionXPos = new List<float> { -11, -8f, -5f, -2f, 1f, 4f, 7f };   //new List<int> {-12, -8, -4, 0, 4, 8, 12 };
    private List<float> cardsInHandXPos = new List<float> { -11, -8f, -5f, -2f, 1f, 4f, 7f };

    private int startingCards = 3;
    public int amountOfCardsInHand = 0;
    private int maxHandSize = 7;

    public int opponentCardsInHand = 0;

    public List<GameObject> cardsInHand = new List<GameObject>();
    public List<GameObject> cardsInOpponentHand = new List<GameObject>();

    public int maxBoardSize = 7;

    private int spawnedMinions = 0;

    public bool pauseInteractions = false;

    public int currentMana = 0;
    private int maxManaThisTurn = 0;
    private int currentTurn = 0;
    private int maxMana = 10;

    public TextMesh manaText;
    public TextMesh cardsText;
    public TextMesh playerText;

    public int playerTurn = 1;

    public GameObject cardHider;
    private List<GameObject> cardHiders = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        questionCreationPanel.SetActive(false);
        questionPanel.SetActive(false);
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

        setCardCovers(1);
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

        float minionX = minionXPos[listID];

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
            minion.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject minion = Instantiate(minionToSpawn, new Vector3(minionX, -2.5f, 0), new Quaternion(0,0,0,0)); //Spawn minion
            aliveMinionsOpponent++;
            minion.transform.parent = playBoard.transform;
            minion.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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

    private void checkQuestions()
    {
        mainPlayerActiveQuestions = 0;
        opponentActiveQuestions = 0;

        foreach(Question item in askedQuestions)
        {
            if(item.answered == false)
            {
                if (item.questionFromPlayer == 1)
                {
                    mainPlayerActiveQuestions++;
                }
                else opponentActiveQuestions++;
            }
        }
    }

    public void showQuestion()
    {
        
        Question questionToSet = null;
        bool setQuestionActive = false;

        if (playerTurn == 1 && opponentActiveQuestions > 0)
        {
            questionToSet = askedQuestions.Find(x => !x.answered && x.questionFromPlayer != playerTurn);
            setQuestionActive = true;
        }
        else if (playerTurn == 2 && mainPlayerActiveQuestions > 0)
        {
            questionToSet = askedQuestions.Find(x => !x.answered && x.questionFromPlayer != playerTurn);
            setQuestionActive = true;
        }

        if (setQuestionActive && questionToSet != null)
        {
            askedQuestion.text = questionToSet.question;
            currentQuestionID = questionToSet.questionID;

            switch (Random.Range(1,4))
            {
                case 1:
                    answerField_1.text = questionToSet.correctAnswer;

                    int firstWrongPos = Random.Range(1, 3); //low is first, higher is end (pos 2 and 3 in this case)
                    if(firstWrongPos == 1)
                    {
                        answerField_2.text = questionToSet.fakeAnswer_1;
                        answerField_3.text = questionToSet.fakeAnswer_2;
                    }
                    else
                    {
                        answerField_2.text = questionToSet.fakeAnswer_2;
                        answerField_3.text = questionToSet.fakeAnswer_1;
                    }
                    break;
                case 2:
                    answerField_2.text = questionToSet.correctAnswer;

                    int firstWrongPos_1 = Random.Range(1, 3); //low is first, higher is end (pos 1 and 3 in this case)
                    if (firstWrongPos_1 == 1)
                    {
                        answerField_1.text = questionToSet.fakeAnswer_1;
                        answerField_3.text = questionToSet.fakeAnswer_2;
                    }
                    else
                    {
                        answerField_1.text = questionToSet.fakeAnswer_2;
                        answerField_3.text = questionToSet.fakeAnswer_1;
                    }
                    break;
                case 3:
                    answerField_3.text = questionToSet.correctAnswer;

                    int firstWrongPos_2 = Random.Range(1, 3); //low is first, higher is end (pos 2 and 3 in this case)
                    if (firstWrongPos_2 == 1)
                    {
                        answerField_1.text = questionToSet.fakeAnswer_1;
                        answerField_2.text = questionToSet.fakeAnswer_2;
                    }
                    else
                    {
                        answerField_1.text = questionToSet.fakeAnswer_2;
                        answerField_2.text = questionToSet.fakeAnswer_1;
                    }
                    break;
            }
            pauseInteractions = true;
            questionPanel.SetActive(true);
        }
    }

    public void answerQuestion(int selection)
    {
        Question currentQuestion = askedQuestions.Find(x => !x.answered && x.questionID == currentQuestionID);

        switch (selection)
        {
            case 1:
                if(answerField_1.text == currentQuestion.correctAnswer)
                {
                    currentQuestion.answered = true;
                    Debug.Log("Correct");
                }
                else
                {
                    currentQuestion.attemptsMade++;
                }
                break;
            case 2:
                if (answerField_2.text == currentQuestion.correctAnswer)
                {
                    currentQuestion.answered = true;
                    Debug.Log("Correct");
                }
                else
                {
                    currentQuestion.attemptsMade++;
                }
                break;
            case 3:
                if (answerField_3.text == currentQuestion.correctAnswer)
                {
                    currentQuestion.answered = true;
                    Debug.Log("Correct");
                }
                else
                {
                    currentQuestion.attemptsMade++;
                }
                break;
        }
        questionPanel.SetActive(false);
        pauseInteractions = false;
    }

    public void submitQuestion()
    {
        if(question.text.Length > 0 && fake_answer_1.text.Length > 0 && fake_answer_2.text.Length > 0 && correct_answer.text.Length > 0)
        {
            pauseInteractions = false;
            amountOfAskedQuestions++;
            askedQuestions.Add(new Question
            {
                questionID = amountOfAskedQuestions,
                question = question.text,
                fakeAnswer_1 = fake_answer_1.text,
                fakeAnswer_2 = fake_answer_2.text,
                correctAnswer = correct_answer.text,
                questionFromPlayer = playerTurn
            });

            question.text = string.Empty;
            fake_answer_1.text = string.Empty;
            fake_answer_2.text = string.Empty;
            correct_answer.text = string.Empty;

            questionCreationPanel.SetActive(false);

            if (playerTurn == 1) mainPlayerActiveQuestions++;
            else opponentActiveQuestions++;
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
            playerText.color = Color.green;
        }
        else
        {
            playerTurn = 1;
            playerText.color = Color.magenta;
            if (currentTurn < maxMana) currentTurn++;
        }

        playerText.text = "Player " + playerTurn;
        

        drawCard(playerTurn);
        setTurn(currentTurn);
       
        int rotation = 0;
        if (playBoard.transform.rotation.x == 0) rotation = 180;
        playBoard.transform.rotation = new Quaternion(rotation, 0, 0, 0);

        setCardCovers(playerTurn);

        showQuestion();
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
                Vector3 newPos = new Vector3(cardsInHandXPos[i], cardsInHand[i].transform.localPosition.y, cardsInHand[i].transform.localPosition.z);
                cardsInHand[i].transform.localPosition = newPos;

                if (cardsInHand[i].tag == "Minion")
                {
                    cardsInHand[i].GetComponent<minionController>().setNewPos(newPos);
                }
                else if (cardsInHand[i].tag == "Spell")
                {
                    cardsInHand[i].GetComponent<spellController>().setNewPos(newPos);
                }
            }
        }
        else
        {
            for (int i = 0; i < cardsInOpponentHand.Count; i++)
            {
                Vector3 newPos = new Vector3(cardsInHandXPos[i], cardsInOpponentHand[i].transform.localPosition.y, cardsInOpponentHand[i].transform.localPosition.z);
                cardsInOpponentHand[i].transform.localPosition = newPos;

                if (cardsInOpponentHand[i].tag == "Minion")
                {
                    cardsInOpponentHand[i].GetComponent<minionController>().setNewPos(newPos);
                }
                else if (cardsInOpponentHand[i].tag == "Spell")
                {
                    cardsInOpponentHand[i].GetComponent<spellController>().setNewPos(newPos);
                }
            }
        }
    }

    private void setCardCovers(int playerTurn)
    {
        for(int i = 0; i < cardHiders.Count; i++)
        {
            Destroy(cardHiders[i].gameObject);
        }
        cardHiders.Clear();

        if (playerTurn == 1)
        {
            for (int i = 0; i < cardsInOpponentHand.Count; i++)
            {
                Vector3 newPos = new Vector3(cardsInHandXPos[i], cardsInOpponentHand[i].transform.localPosition.y, cardsInOpponentHand[i].transform.localPosition.z);

                GameObject tempHider = Instantiate(cardHider, new Vector3(cardsInHandXPos[i], 7.5f, 0), new Quaternion(0, 0, 0, 0)); //Spawn hider

                cardHiders.Add(tempHider);
            }
        }
        else if(playerTurn == 2)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                Vector3 newPos = new Vector3(cardsInHandXPos[i], cardsInHand[i].transform.localPosition.y, cardsInHand[i].transform.localPosition.z);

                GameObject tempHider = Instantiate(cardHider, new Vector3(cardsInHandXPos[i], 7.5f, 0), new Quaternion(0, 0, 0, 0)); //Spawn hider

                cardHiders.Add(tempHider);
            }
        }
    }

    public void reduceMana(int amount)
    {
        currentMana -= amount;
        manaText.text = "Mana: " + currentMana + "/" + maxManaThisTurn;
    }
}
