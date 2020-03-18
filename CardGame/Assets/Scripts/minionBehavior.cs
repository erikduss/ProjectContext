using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionBehavior : MonoBehaviour
{
    public int minionID;
    public int health;
    public bool played = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0 && played)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(this.gameObject);
    }
}
