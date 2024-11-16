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
        if (current == TimeOfDay.Dawn && currentTime <= endDawnTime)
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime * realTimeToGameTimeMultiplier);
            timeText.enabled = true;
            timeText.text = currentTime.ToString("HH:mm");

            float t = Mathf.InverseLerp(initDawnTime, 5f, (float)currentTime.TimeOfDay.TotalHours);

            light.color = Color.Lerp(new Color(0f, 0f, 0f), new Color(1f, 0.7f, 1f), t);
            light.colorTemperature = Mathf.Lerp(20000f, 1500f, t);
            light.intensity = Mathf.Lerp(0f, 0.8f, t);
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

        // (원하는 색상 RGB 값) / 255 계산기로 계산함 ㅎㅎ..
        filter = new Color(1f, 1f, 0.8f);      
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
    }
}