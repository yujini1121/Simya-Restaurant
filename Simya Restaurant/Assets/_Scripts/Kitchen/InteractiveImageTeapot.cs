using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveImageTeapot : InteractiveImageBase
{
    public static InteractiveImageTeapot instance;
    [SerializeField] public Sprite TeaAdded;

    private void Awake()
    {
        instance = this;
    }

    public void AddPetal()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = TeaAdded;
    }
}
