using System;
using UnityEngine;

[System.Serializable]
public class Drum : MonoBehaviour
{
    private float rotationVelocity;
    public SlotElement currentElement;

    public AudioClip[] sounds = new AudioClip[0];

    [SerializeField]private Animator animator;
    [SerializeField]private AudioSource audioSource;

    public Action<Drum> finishedCallback;

    private bool needStop;
    public bool IsFinished { get; private set; }
    public SlotElement CurrentElement
    {
        get => currentElement;
        set => currentElement = value;
    }


    private void OnEnable ()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentElement = SlotElement.Fist;
        animator.speed = 0;
        needStop = false;
        IsFinished = true;
    }

    public void StartRotation (float velocity)
    {
        IsFinished = false;
        rotationVelocity = velocity;
        animator.speed = rotationVelocity;
    }

    public void StopRotation ()
    {
        animator.speed = 0;
        rotationVelocity = 0f;
        IsFinished = true;
        finishedCallback?.Invoke(this);
    }

    public void Rotate ()
    {
        if (IsFinished)
        {
            return;
        }

        rotationVelocity = Mathf.LerpUnclamped(rotationVelocity, 0, Time.deltaTime);
        animator.speed = rotationVelocity;

        if (rotationVelocity < 0.4f)
        {
            needStop = true;
        }
    }


    public void SetElementType (SlotElement slotElement)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        CurrentElement = slotElement;
    }

    public void OnStopElement ()
    {
        if (needStop)
        {
            needStop = false;
            StopRotation();
        }
    }
}
