using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
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
            textScore.text = $"Score: {score}, Win/Lose: {wins}/{loses}, Spins: {wins+loses}" ;
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
            if (score >= 10)
            {
                Score -= 10;
            }

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

        if ((slotsCombination[0] == slotsCombination[1]) && slotsCombination[1] == slotsCombination[2])
        {
            textStatus.text = "<color=green>You win";
            wins++;
            audioWin.Play();
            switch (slotsCombination[0])
            {
                case SlotElement.Fist:
                    Score += 100;
                    break;
                case SlotElement.Ammo:
                    Score += 20;
                    break;
                case SlotElement.Bomb:
                    Score += 40;
                    break;
                case SlotElement.Knife:
                    Score += 80;
                    break;
            }
        }
        else
        {
            loses++;
            textStatus.text = "<color=red>You lose";
            textScore.text = $"Score: {score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
        }
    }
}