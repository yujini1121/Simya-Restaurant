using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] private Slider masterVol, musicVol, sfxVol;
    [SerializeField] private AudioMixer TestAudioMixer;
    [SerializeField] private Image masterVolImage, musicVolImage, sfxVolImage;

    public void ControllerMasterVolume()
    {
        TestAudioMixer.SetFloat("MasterVol", masterVol.value);
    }

    public void ControllerMusicVolume()
    {
        TestAudioMixer.SetFloat("MusicVol", musicVol.value);
    }

    public void ControllerSFXVolume()
    {
        TestAudioMixer.SetFloat("SFXVol", sfxVol.value);
    }
}
