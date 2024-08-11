using System;
using System.Collections;
using UnityEngine;
using TMPro;

public enum TimeOfDay
{
    Day,
    Night,
    Dawn
}

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TimeOfDay current;
    [SerializeField] private TextMeshProUGUI timeText;

    [Space(20)]
    [Tooltip("현실 시간과 게임 시간 간의 변환 비율")]
    [SerializeField] private float realTimeToGameTimeMultiplier;

    [SerializeField] private float initGameTime;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    private DateTime currentTime;
    private TimeSpan dayTimeSpan;
    private TimeSpan nightTimeSpan;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private Color dawnAmbientLight;

    [SerializeField] private AnimationCurve lightChangeCurve;


    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(initGameTime);

        dayTimeSpan = TimeSpan.FromHours(sunriseHour);
        nightTimeSpan = TimeSpan.FromHours(sunsetHour);
    }

    private void Update()
    {
        if (current == TimeOfDay.Night)
        {
            timeText.enabled = true;

            UpdateTime();
            RotateSun();
        }
        else
        {
            timeText.enabled = false;
        }
    }

    private void UpdateTime()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * realTimeToGameTimeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    /// <summary>
    /// case 1. 새벽 -> 낮 ///
    /// case 2. 낮 -> 밤
    /// </summary>
    private void SkipToNextTime(TimeOfDay current)
    {
        if      (current == TimeOfDay.Dawn)
        {

        }
        else if (current == TimeOfDay.Day)
        {

        }
        else
        {
            Debug.LogError("SkipToNextTime() - Not Found Current Time");
            return;
        }
    }

    /// <summary>
    /// 밤 -> 새벽 
    /// </summary>
    private void DurationToNextTime(TimeOfDay current)
    {
        if (current == TimeOfDay.Night)
        {
            
        }
        else
        {
            Debug.LogError("DurationToNextTime() - Not Found Current Time");
            return;
        }
    }


    private void RotateSun()
    {
        Debug.Log("Rotate Sun");

        float lightLerp;

        if (currentTime.TimeOfDay > dayTimeSpan && currentTime.TimeOfDay < nightTimeSpan)
        {
            TimeSpan timeSinceSunrise = CalculateTime(dayTimeSpan, currentTime.TimeOfDay);
            TimeSpan sunriseToSunsetDuration = CalculateTime(dayTimeSpan, nightTimeSpan);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            lightLerp = Mathf.Lerp(200, 360, (float)percentage);
            transform.rotation = Quaternion.AngleAxis(lightLerp, Vector3.right);

            float dotProduct = Vector3.Dot(transform.forward, Vector3.down);
            float intensityLerp = Mathf.Lerp(0, 1, lightChangeCurve.Evaluate(dotProduct));

            GetComponent<Light>().intensity = intensityLerp;
        }
        else
        {
            SetDawnToDay();
        }

    }

    private TimeSpan CalculateTime(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0.1f)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }


    #region Set Time Methods
    public void SetDawnToDay()
    {
        Debug.Log("Set Dawn");

        current = TimeOfDay.Dawn;
        SkipToNextTime(current);
    }

    public void SetDayToNight()
    {
        Debug.LogWarning("Save All Data");
        Debug.Log("Set Day");

        current = TimeOfDay.Day;
        SkipToNextTime(current);
    }

    public void SetNightToDawn()
    {
        Debug.Log("Set Night");

        current = TimeOfDay.Night;
        DurationToNextTime(current);
    }
    #endregion

}