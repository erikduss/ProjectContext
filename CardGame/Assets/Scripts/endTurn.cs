using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTurn : MonoBehaviour
{
    private GameManager gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        gameController.endTurn();
    }
}
