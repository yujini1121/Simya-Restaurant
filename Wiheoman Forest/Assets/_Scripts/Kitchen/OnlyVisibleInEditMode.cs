using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyVisibleInEditMode : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.color = Color.clear;
        }
        else
        {
            Debug.LogError($"{gameObject.name} 의 이미지가 존재하지 않음");
        }
    }
}
