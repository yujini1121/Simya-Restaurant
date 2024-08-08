using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private TimeSpan dawnTimeSpan;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private Color dawnAmbientLight;

    [SerializeField] private AnimationCurve lightChangeCurve;

    //// 차례대로 새벽, 낮, 밤 시간을 나타냄
    //private float dawnTime;             // 새벽 5시
    //private float noonTime;             // 낮 12시
    //private float midnightTime;         // 저녁 7시 (편의상)


    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(initGameTime);

        dayTimeSpan = TimeSpan.FromHours(sunriseHour);
        nightTimeSpan = TimeSpan.FromHours(sunsetHour);
    }

    private void Update()
    {
        UpdateTime();
        RotateSun();
    }

    private void SkipToNextTime(TimeOfDay current)
    {
        if (current == TimeOfDay.Day)
        {

        }
        else if (current == TimeOfDay.Night)
        {

        }
        else
        {
            Debug.LogError("SkipToNextTime() - Not Found Current Time");
            return;
        }
    }

    private void DurationToNextTime()
    {
        //if (current == TimeOfDay.Dawn)
        //{

        //}
        //else
        //{
        //    Debug.LogError("DurationToNextTime() - Not Found Current Time");
        //    return;
        //}
    }

    private void UpdateTime()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * realTimeToGameTimeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLight;

        if (currentTime.TimeOfDay > dayTimeSpan && currentTime.TimeOfDay < nightTimeSpan)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTime(dayTimeSpan, nightTimeSpan);
            TimeSpan timeSinceSunrise = CalculateTime(dayTimeSpan, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLight = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTime(nightTimeSpan, dayTimeSpan);
            TimeSpan timeSinceSunset = CalculateTime(nightTimeSpan, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLight = Mathf.Lerp(180, 360, (float)percentage);
        }

        GetComponent<Light>().transform.rotation = Quaternion.AngleAxis(sunLight, Vector3.right);
    }


    private TimeSpan CalculateTime(TimeSpan from, TimeSpan to)
    {
        TimeSpan difference = to - from;

        if (difference.TotalSeconds < 0.1f)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }



    #region Set Time Methods
    public void SetDay()
    {
        Debug.Log("Set Day");

        current = TimeOfDay.Day;
        SkipToNextTime(current);
    }

    public void SetNight()
    {
        Debug.Log("Set Night");

        current = TimeOfDay.Night;
        SkipToNextTime(current);
    }

    public void SetDawn()
    {
        Debug.Log("Set Dawn");

        current = TimeOfDay.Dawn;
        DurationToNextTime();
    }
    #endregion


    //private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    //{
    //    TimeSpan difference = toTime - fromTime;

    //    if (difference.TotalSeconds < 0)
    //    {
    //        difference += TimeSpan.FromHours(24);
    //    }

    //    return difference;
    //}

}