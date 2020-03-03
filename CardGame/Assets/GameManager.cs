﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> minions;
    public List<Card> minionsData;

    public List<GameObject> activeMinions = new List<GameObject>();

    public int alivePlayerMinions = 0;

    public List<BoardSpot> mainPlayerBoard = new List<BoardSpot>();
    public List<BoardSpot> opponentBoard = new List<BoardSpot>();

    private List<int> minionXPos = new List<int> {-12, -8, -4, 0, 4, 8, 12 };

    public int maxBoardSize = 7;

    private int spawnedMinions = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< maxBoardSize; i++)
        {
            mainPlayerBoard.Add(new BoardSpot(i));
            opponentBoard.Add(new BoardSpot(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonMinion(string minionName)
    {
        int listID = mainPlayerBoard.FindIndex(a => a.Occupied == false);

        GameObject minionToSpawn = minions.Find(x => x.name == minionName);
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
}
