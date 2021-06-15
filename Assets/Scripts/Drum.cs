using System;
using UnityEngine;

[System.Serializable]
public class Drum : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _sounds;

    public event Action<Drum> Finished;

    private float _rotationVelocity;
    private SlotElement _currentElement;

    private bool _needStop;
    public bool IsFinished { get; private set; }
    public SlotElement CurrentElement
    {
        get => _currentElement;
        set => _currentElement = value;
    }


    private void OnEnable ()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentElement = SlotElement.Fist;
        _animator.speed = 0;
        _needStop = false;
        IsFinished = true;
    }

    public void StartRotation (float velocity)
    {
        IsFinished = false;
        _rotationVelocity = velocity;
        _animator.speed = _rotationVelocity;
    }

    public void StopRotation ()
    {
        _animator.speed = 0;
        _rotationVelocity = 0f;
        IsFinished = true;
        Finished?.Invoke(this);
    }

    public void Rotate ()
    {
        if (IsFinished)
        {
            return;
        }

        _rotationVelocity = Mathf.LerpUnclamped(_rotationVelocity, 0, Time.deltaTime);
        _animator.speed = _rotationVelocity;

        if (_rotationVelocity < 0.4f)
        {
            _needStop = true;
        }
    }

    //Animation event
    public void SetElementType (SlotElement slotElement)
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        CurrentElement = slotElement;
    }


    //Animation event
    public void OnStopElement ()
    {
        if (_needStop)
        {
            _needStop = false;
            StopRotation();
        }
    }
}
