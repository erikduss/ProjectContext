using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> minions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonMinion(string minionName)
    {
        GameObject minionToSpawn = minions.Find(x => x.name == minionName);
        Instantiate(minionToSpawn, new Vector3(0,0,0), Quaternion.identity);
    }
}
