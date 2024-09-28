using System;
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
    #region 아 다시 짜야지
    //[SerializeField] private TimeOfDay current;
    //[SerializeField] private TextMeshProUGUI timeText;

    //[Space(20)]
    //[Tooltip("현실 시간과 게임 시간 간의 변환 비율")]
    //[SerializeField] private float realTimeToGameTimeMultiplier;

    //[SerializeField] private float initGameTime;
    //[SerializeField] private float sunriseHour;
    //[SerializeField] private float sunsetHour;

    //private DateTime currentTime;
    //private TimeSpan dayTimeSpan;
    //private TimeSpan nightTimeSpan;

    //[SerializeField] private Color dayAmbientLight;
    //[SerializeField] private Color nightAmbientLight;
    //[SerializeField] private Color dawnAmbientLight;

    //[SerializeField] private AnimationCurve lightChangeCurve;


    //private void Start()
    //{
    //    currentTime = DateTime.Now.Date + TimeSpan.FromHours(initGameTime);

    //    dayTimeSpan = TimeSpan.FromHours(sunriseHour);
    //    nightTimeSpan = TimeSpan.FromHours(sunsetHour);
    //}

    //private void Update()
    //{
    //    if (current == TimeOfDay.Night)
    //    {
    //        timeText.enabled = true;
    //        UpdateTime();
    //    }
    //    else
    //    {
    //        timeText.enabled = false;
    //    }
    //}

    //private void UpdateTime()
    //{
    //    currentTime = currentTime.AddSeconds(Time.deltaTime * realTimeToGameTimeMultiplier);

    //    if (timeText != null)
    //    {
    //        timeText.text = currentTime.ToString("HH:mm");
    //    }
    //}

    ///// <summary>
    ///// case 1. 새벽 -> 낮 ///
    ///// case 2. 낮 -> 밤
    ///// </summary>
    //private void SkipToNextTime(TimeOfDay current)
    //{
    //    if      (current == TimeOfDay.Dawn)
    //    {

    //    }
    //    else if (current == TimeOfDay.Day)
    //    {

    //    }
    //    else
    //    {
    //        Debug.LogError("SkipToNextTime() - Not Found Current Time");
    //        return;
    //    }
    //}

    ///// <summary>
    ///// 밤 -> 새벽 
    ///// </summary>
    //private void DurationToNextTime(TimeOfDay current)
    //{
    //    if (current == TimeOfDay.Night)
    //    {

    //    }
    //    else
    //    {
    //        Debug.LogError("DurationToNextTime() - Not Found Current Time");
    //        return;
    //    }
    //}
    #endregion
    //IEnumerator ChangeNextTime()
    //{
    //    switch (current)
    //    {
    //        case TimeOfDay.Dawn:    DawnToDay();    break;
    //        case TimeOfDay.Day:     DayToNight();   break;
    //        case TimeOfDay.Night:   NightToDawn();  break;
    //    }

    //    yield return null;
    //}

    [Header("Current")]
    [SerializeField] private TimeOfDay current;

    [SerializeField] private Light light;
    [SerializeField] private Color filter;
    [SerializeField] private float temperature;
    [SerializeField] private float intensity;

    [Header("Night To Dawn")]
    [Tooltip("현실 시간과 게임 시간 간의 변환 비율")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float realTimeToGameTimeMultiplier;
    [SerializeField] private DateTime currentTime;
    [SerializeField] private DateTime endDawnTime;
    [SerializeField] private float initDawnTime;



    private void Awake()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(initDawnTime);
        endDawnTime = DateTime.Now.Date + TimeSpan.FromHours(5f);

        light = gameObject.GetComponent<Light>();

        filter = light.color;
        temperature = light.colorTemperature;
        intensity = light.intensity;
    }

    private void Update()
    {
        Debug.Log(currentTime.ToString());
        if (current == TimeOfDay.Dawn && currentTime <= endDawnTime)
        {
            timeText.enabled = true;

            currentTime = currentTime.AddSeconds(Time.deltaTime * realTimeToGameTimeMultiplier);

            if (timeText != null)
            {
                timeText.text = currentTime.ToString("HH:mm");
            }
        }
        else
        {
            timeText.enabled = false;
            current = TimeOfDay.Day;
        }    
    }

    /// <summary>
    /// Skip 
    /// </summary>
    public void DawnToDay()
    {
        Debug.Log("Dawn To Day");
        current = TimeOfDay.Day;

        filter = new Color(1f, 1f, 0.8f);           // (원하는 색상 RGB 값) / 255 
        temperature = 6500f;
        intensity = 1f;

        light.color = filter;
        light.colorTemperature = temperature;
        light.intensity = intensity;
    }

    /// <summary>
    /// Skip  
    /// </summary>
    public void DayToNight()
    {
        Debug.Log("Day To Night");
        current = TimeOfDay.Night;

        filter = new Color(0f, 0f, 0f);
        temperature = 20000f;
        intensity = 0f;

        light.color = filter;
        light.colorTemperature = temperature;
        light.intensity = intensity;
    }

    /// <summary>
    /// Duration  
    /// </summary>
    public void NightToDawn()
    {
        Debug.Log("Night To Dawn");
        current = TimeOfDay.Dawn;

        filter = new Color(1f, 0.7f, 1f);
        temperature = 1500f;
        intensity = 0.8f;

        light.color = filter;
        light.colorTemperature = temperature;
        light.intensity = intensity;
    }
}