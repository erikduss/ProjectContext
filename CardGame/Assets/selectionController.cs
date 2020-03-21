using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectionController : MonoBehaviour
{
    public Toggle answer1;
    public Toggle answer2;
    public Toggle answer3;

    public Button btn_confirmSelection;

    private int lastToggled = 0;

    private GameManager gamecontroller;

    // Start is called before the first frame update
    void Start()
    {
        gamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        btn_confirmSelection.onClick.AddListener(confirmSelection);
    }

    // Update is called once per frame
    void Update()
    {
        if (answer1.isOn && lastToggled != 1)
        {
            answer2.isOn = false;
            answer3.isOn = false;
            lastToggled = 1;
        }
        if (answer2.isOn && lastToggled != 2)
        {
            answer1.isOn = false;
            answer3.isOn = false;
            lastToggled = 2;
        }
        if (answer3.isOn && lastToggled != 3)
        {
            answer2.isOn = false;
            answer1.isOn = false;
            lastToggled = 3;
        }
    }

    public void confirmSelection()
    {
        if (answer1.isOn)
        {
            gamecontroller.answerQuestion(1);
        }
        else if (answer2.isOn)
        {
            gamecontroller.answerQuestion(2);
        }
        else if (answer3.isOn)
        {
            gamecontroller.answerQuestion(3);
        }

        answer1.isOn = false;
        answer2.isOn = false;
        answer3.isOn = false;
    }
}
