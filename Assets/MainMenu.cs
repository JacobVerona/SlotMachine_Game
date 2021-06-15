using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public SlotMachine slotMachine;

    public void OnEnable ()
    {
        Pause();
    }

    public void OnStart ()
    {
        slotMachine.Audior.outputAudioMixerGroup.audioMixer.SetFloat("Volume", 0);
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void OnExit ()
    {
        Application.Quit();
    }

    private void Pause ()
    {
        slotMachine.Audior.outputAudioMixerGroup.audioMixer.SetFloat("Volume", -60f);
        mainMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
