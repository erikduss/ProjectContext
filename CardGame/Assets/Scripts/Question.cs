using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public int questionID;
    public string question;
    public string fakeAnswer_1;
    public string fakeAnswer_2;
    public string correctAnswer;

    public bool answered = false;
    public int questionFromPlayer;
    public int attemptsMade = 0;
}
