using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    private static readonly SlotElement[,,] combinations = new SlotElement[,,]
    {
        { {SlotElement.Ammo }, { SlotElement.Ammo }, { SlotElement.Ammo } },
        { {SlotElement.Bomb }, { SlotElement.Bomb }, { SlotElement.Bomb } },
        { {SlotElement.Fist }, { SlotElement.Fist }, { SlotElement.Fist } },
        { {SlotElement.Knife }, { SlotElement.Knife }, { SlotElement.Knife } },
    };

    private static readonly int[] winList = new int[]
    {
        200, 200, 200, 200
    };

    public Drum[] drums = new Drum[3];
    public Animation animator;
    public AudioSource audior;
    public AudioSource audioWin;
    public TMP_Text[] text = new TMP_Text[3];
    public TMP_Text textScore;
    public TMP_Text textStatus;

    [SerializeField] private bool isStarted;

    public int score = 100;

    private int wins;
    private int loses;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (score < 0)
            {
                textScore.text = $"Debt: {-score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
            }
            else
            {
                textScore.text = $"Score: {score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
            }
        }
    }
    
    public void OnEnable ()
    {
        textScore.text = $"Score: {score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
        foreach (var drum in drums)
        {
            drum.finishedCallback += OnDrumFinished;
        }
    }

    public void FixedUpdate ()
    {
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && !isStarted)
        {
            Score -= 10;
            

            int multiply = 1;
            foreach (var drum in drums)
            {
                audior.Play();
                animator.Play();
                drum.StartRotation(UnityEngine.Random.Range(50f,100f) * ++multiply);
                isStarted = true;
            }
        }
        foreach (var drum in drums)
        {
            drum.Rotate();
            for (int i = 0; i < drums.Length; i++)
            {
                text[i].text = drums[i].CurrentElement.ToString();
            }
        }
    }

    public void OnDrumFinished (Drum drum)
    {
        for (int i = 0; i < drums.Length; i++)
        {
            if (!drums[i].IsFinished)
            {
                return;
            }
        }

        isStarted = false;
        SlotElement[] slotsCombination = new SlotElement[drums.Length];
        for (int i = 0; i < drums.Length; i++)
        {
            slotsCombination[i] = drums[i].CurrentElement;
            text[i].text = drums[i].CurrentElement.ToString();
        }

        for (int i = 0; i < combinations.GetLength(0); i++)
        {
            if (slotsCombination[0] == combinations[i, 0, 0] && slotsCombination[1] == combinations[i, 1, 0] && slotsCombination[2] == combinations[i, 2, 0])
            {
                textStatus.text = "<color=green>You win";
                wins++;
                audioWin.Play();
                Score += winList[i];
                return;
            }
        }
        loses++;
        textStatus.text = "<color=red>You lose";
        Score += 0;
    }
}