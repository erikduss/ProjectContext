using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpot
{
    //The min position of a board spot is -12 and the max position is 12
    //Every card is 4 apart

        // 1 minion: 0
        // 2 minions: -2 , 2
        // 3 minoins: -4, 0, 4
        // 4 minions: -6, -2, 2, 6
        // 5 minions: -8, -4, 0, 4, 8
        // 6 minions: -10, -6, -2, 2, 6, 10
        // 7 minions: -12, -8, -4, 0, 4, 8, 12

    private bool occupied;
    public bool Occupied
    {
        get { return occupied; }
        set { occupied = value; }
    }

    private int attackMinion;
    public int AttackMinion
    {
        get { return attackMinion; }
        set { attackMinion = value; }
    }

    private int maxHealthMinion;
    public int MaxHealthMinion
    {
        get { return maxHealthMinion; }
        set { maxHealthMinion = value; }
    }

    private int currentHealthMinion;
    public int CurrentHealthMinion
    {
        get { return currentHealthMinion; }
        set { currentHealthMinion = value; }
    }

    private int boardSpotID;
    public int BoardSpotID
    {
        get { return boardSpotID; }
        set { boardSpotID = value; }
    }

    // Start is called before the first frame update
    public BoardSpot(int spotID)
    {
        occupied = false;
        attackMinion = 0;
        maxHealthMinion = 0;
        currentHealthMinion = 0;
        boardSpotID = spotID;
    }

    public void placeMinion(int attack, int maxHealth, int boardID)
    {
        occupied = true;
        attackMinion = attack;
        maxHealthMinion = maxHealth;
        currentHealthMinion = maxHealth;
        boardSpotID = boardID;
    }

    public void killMinion()
    {
        occupied = false;
        attackMinion = 0;
        maxHealthMinion = 0;
        currentHealthMinion = 0;
    }
}
