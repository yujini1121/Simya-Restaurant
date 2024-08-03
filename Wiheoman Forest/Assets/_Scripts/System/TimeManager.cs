using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeOfDay
{
    Day,
    Night,
    Dawn
}

public class TimeManager : MonoBehaviour
{
    #region variable
    public TimeOfDay current;

    [SerializeField]
    private float timeMultipliler;

    [SerializeField]
    private float startTime;

    // 차례대로 새벽, 낮, 밤 시간을 나타냄
    private float dawnTime;          // 새벽 5시
    private float noonTime;          // 낮 12시
    private float midnightTime;      // 저녁 7시 (편의상)
    #endregion


    private void Start()
    {

    }


    private void DurationToNextTime(TimeOfDay current)
    {

    }

    private void SkipToNextTime(TimeOfDay current)
    {
        if (current == TimeOfDay.Day)
        {
            Debug.Log("Day!!!!!!!!");
        }
        else if (current == TimeOfDay.Night)
        {
            Debug.Log("Night!!!!!!!!!!");
        }    
    }



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
        DurationToNextTime(current);
    }

    public void SetDawn()
    {
        Debug.Log("Set Dawn");

        current = TimeOfDay.Dawn;
        SkipToNextTime(current);
    }


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