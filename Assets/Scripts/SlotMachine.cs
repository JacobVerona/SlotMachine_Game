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

    [SerializeField] private Drum[] _drums = new Drum[3];
    [SerializeField] private Animation _animator;
    [SerializeField] private AudioSource _audior;
    [SerializeField] private AudioSource _audioWin;
    [SerializeField] private TMP_Text[] _text = new TMP_Text[3];
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textStatus;

    [SerializeField] private bool _isStarted;

    private int _score = 100;
    private int wins;
    private int loses;

    public AudioSource Audior { get => _audior; }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            if (_score < 0)
            {
                _textScore.text = $"Debt: {-_score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
            }
            else
            {
                _textScore.text = $"Score: {_score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
            }
        }
    }
    
    public void OnEnable ()
    {
        _textScore.text = $"Score: {_score}, Win/Lose: {wins}/{loses}, Spins: {wins + loses}";
        foreach (var drum in _drums)
        {
            drum.Finished += OnDrumFinished;
        }
    }

    public void FixedUpdate ()
    {
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && !_isStarted)
        {
            Score -= 10;
            

            int multiply = 1;
            foreach (var drum in _drums)
            {
                _audior.Play();
                _animator.Play();
                drum.StartRotation(UnityEngine.Random.Range(50f,100f) * ++multiply);
                _isStarted = true;
            }
        }
        foreach (var drum in _drums)
        {
            drum.Rotate();
            for (int i = 0; i < _drums.Length; i++)
            {
                _text[i].text = _drums[i].CurrentElement.ToString();
            }
        }
    }

    public void OnDrumFinished (Drum drum)
    {
        for (int i = 0; i < _drums.Length; i++)
        {
            if (!_drums[i].IsFinished)
            {
                return;
            }
        }

        _isStarted = false;
        SlotElement[] slotsCombination = new SlotElement[_drums.Length];
        for (int i = 0; i < _drums.Length; i++)
        {
            slotsCombination[i] = _drums[i].CurrentElement;
            _text[i].text = _drums[i].CurrentElement.ToString();
        }

        for (int i = 0; i < combinations.GetLength(0); i++)
        {
            if (slotsCombination[0] == combinations[i, 0, 0] && slotsCombination[1] == combinations[i, 1, 0] && slotsCombination[2] == combinations[i, 2, 0])
            {
                _textStatus.text = "<color=green>You win";
                wins++;
                _audioWin.Play();
                Score += winList[i];
                return;
            }
        }
        loses++;
        _textStatus.text = "<color=red>You lose";
        Score += 0;
    }
}