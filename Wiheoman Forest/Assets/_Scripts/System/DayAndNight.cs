using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeOfDay
{
    Day,
    Night,
    Dawn
}

public class DayAndNight : MonoBehaviour
{
    public TimeOfDay current;

    [Header("Day")]
    public Light day;
    public Color dayColor;

    [Header("Night")]
    public Light night;
    public Color nightColor;

    [Header("Dawn")]
    public Light dawn;
    public Color dawnColor;


    private void Start()
    {
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        switch (current)
        {
            case TimeOfDay.Day:
                day.color = dayColor;
                day.gameObject.SetActive(true);
                night.gameObject.SetActive(false);
                dawn.gameObject.SetActive(false);
                break;

            case TimeOfDay.Night:
                night.color = nightColor;
                night.gameObject.SetActive(true);
                day.gameObject.SetActive(false);
                dawn.gameObject.SetActive(false);
                break;

            case TimeOfDay.Dawn:
                dawn.color = dawnColor;
                dawn.gameObject.SetActive(true);
                day.gameObject.SetActive(false);
                night.gameObject.SetActive(false);
                break;
        }
    }

    public void SetDay()
    {
        current = TimeOfDay.Day;
        UpdateLighting();
    }

    public void SetNight()
    {
        current = TimeOfDay.Night;
        UpdateLighting();
    }

    public void SetDawn()
    {
        current = TimeOfDay.Dawn;
        UpdateLighting();
    }
}