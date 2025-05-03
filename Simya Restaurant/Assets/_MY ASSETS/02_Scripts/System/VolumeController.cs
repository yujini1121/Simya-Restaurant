using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [Header("Volume Slider")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    [Header("Volume Text")]
    [SerializeField] private TextMeshProUGUI[] VolTextList;

    private Slider currentSlider;
    private float val = 0.05f;

    private enum VolumeType
    {
        Master,
        BGM,
        SFX
    }

    private VolumeType selectedVol = VolumeType.Master;

    // Start is called before the first frame update
    void Start()
    {
        currentSlider = masterVolSlider;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchVol(true);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchVol(false);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeVolValue(val);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeVolValue(-val);
        }
    }

    private void SwitchVol(bool up)
    {
        VolumeType previousVol = selectedVol;

        selectedVol = (VolumeType)(((int)selectedVol + (up ? 2 : 1)) % 3);

        VolTextList[(int)previousVol].fontStyle = FontStyles.Normal;

        switch (selectedVol)
        {
            case VolumeType.Master:
                currentSlider = masterVolSlider;
                break;
            case VolumeType.BGM:
                currentSlider = bgmVolSlider;
                break;
            case VolumeType.SFX:
                currentSlider = sfxVolSlider;
                break;
        }

        VolTextList[(int)selectedVol].fontStyle = FontStyles.Bold;
    }

    private void ChangeVolValue(float value)
    {
        if(currentSlider != null)
        {
            currentSlider.value = Mathf.Clamp(currentSlider.value + value, 0.001f, 1f);
            ApplyVol();
        }
    }

    private void ApplyVol()
    {
        AudioManager.instance.SetMasterVolume(masterVolSlider.value);
        AudioManager.instance.SetBgmVolume(bgmVolSlider.value);
        AudioManager.instance.SetSfxVolume(sfxVolSlider.value);
    }
}
