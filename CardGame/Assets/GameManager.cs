using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> minions;
    public List<Card> minionsData;

    public int alivePlayerMinions = 0;

    public List<BoardSpot> mainPlayerBoard = new List<BoardSpot>();
    public List<BoardSpot> opponentBoard = new List<BoardSpot>();

    private List<int> minionXPos = new List<int> {-12, -8, -4, 0, 4, 8, 12 };

    public int maxBoardSize = 7;

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

        mainPlayerBoard[listID].placeMinion(minionData.attack, minionData.health, alivePlayerMinions);

        Instantiate(minionToSpawn, new Vector3(minionX, 0, 0), Quaternion.identity);
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
