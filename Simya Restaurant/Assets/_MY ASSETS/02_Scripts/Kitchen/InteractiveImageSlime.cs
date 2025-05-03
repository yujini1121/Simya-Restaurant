using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageSlime : InteractiveImageBase
{
    [SerializeField] float squeezeTime;
    [SerializeField] Sprite beforeSqueeze;
    [SerializeField] Sprite Squeezing;
    float pressedTime = 0.0f;
    UnityEngine.UI.Image myImage;


    private void Awake()
    {
        if (pressedTime <= 0.0f)
        {
            pressedTime = 1.0f;
            Debug.LogWarning($"InteractiveImageSlime : 미할당 변수 경고, squeezeTime 값이 너무 작아서({squeezeTime}) 1.0f으로 변경합니다.");
        }
    }

    private void Start()
    {
        myImage = GetComponent<UnityEngine.UI.Image>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        pressedTime = Time.time;
        myImage.sprite = Squeezing;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        myImage.sprite = beforeSqueeze;

        if (squeezeTime <= Time.time - pressedTime)
        {
            Debug.Log("슬라임 체액 획득!");
        }
        else
        {
            Debug.Log("스퀴즈 시간이 너무 짧음");
        }
    }
}
